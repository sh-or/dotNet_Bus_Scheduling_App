﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
using DO;
using DS;

//clone!!!

namespace DL
{
    public class DalObject  : IDAL
    {
        public void reset() { }
        #region singelton
        class Nested
        {
            static Nested() { }
            internal static readonly IDAL instance = new DalObject();
        }
        static DalObject() { }
        DalObject() { }
        public static IDAL Instance { get { return Nested.instance; } }
        #endregion

        #region Bus
        public Bus GetBus(int _LicenseNumber)
        {
            Bus b = DataSource.AllBuses.Find(x => x.IsExist && x.LicenseNumber == _LicenseNumber);
            if(b!=null)
                return b.Clone();
            throw new DOException( $"Bus number {_LicenseNumber} was not found");
        }
        public void UpdateBus(Bus b) 
        {
            int n = DataSource.AllBuses.FindIndex(x => x.LicenseNumber == b.LicenseNumber);
            DataSource.AllBuses[n] = b.Clone();
        }
        public IEnumerable<Bus> GetSpecificBuses(Predicate<Bus> p) 
        {
            var ListBS = (from Bus b in DataSource.AllBuses
                               where p(b)
                               select b.Clone());
            if(ListBS!=null)
                return ListBS;
            throw new DOException("No exist buses were found");
        }
        public IEnumerable<Bus> GetAllBuses() 
        {
            var ListBS = (from Bus b in DataSource.AllBuses
                         where b.IsExist
                          select b.Clone());
            if (ListBS != null)
                return ListBS;
            throw new DOException("No buses were found");
        }
        public void AddBus(Bus b)
        {  
            if (DataSource.AllBuses.Exists(x => x.IsExist && x.LicenseNumber == b.LicenseNumber))
                throw new DOException($"Bus number {b.LicenseNumber} is already exist");
            DataSource.AllBuses.Add(b.Clone());
        }
        public void DeleteBus(int _LicenseNumber)
        {
            int n = DataSource.AllBuses.FindIndex(x => x.LicenseNumber == _LicenseNumber);
            if (n > -1) 
                DataSource.AllBuses[n].IsExist = false;
            else
                throw new DOException($"Bus number {_LicenseNumber} was not found");
        }
        #endregion

        #region Bus Station
        public BusStation GetBusStation(int _StationCode) 
        {
            BusStation bs = DataSource.AllBusStations.Find(x => x.IsExist&& x.StationCode == _StationCode);
            if (bs!=null)
                return bs.Clone();
            throw new DOException( $"Bus station number {_StationCode} was not found");
        }
        public void UpdateStation(BusStation bs)
        {
            int n = DataSource.AllBusStations.FindIndex(x => x.StationCode == bs.StationCode);
            if(n>-1)
            DataSource.AllBusStations[n] = bs.Clone();
            else
            throw new DOException($"Bus station number {bs.StationCode} was not found");
        }
        public IEnumerable<BusStation> GetSpecificBusStations(Predicate<BusStation> p) 
        {
            var ListBS = (from BusStation bs in DataSource.AllBusStations
                         where p(bs)&&bs.IsExist
                         select bs.Clone());
            if (ListBS != null)
                return ListBS;
            throw new DOException("No exist bus stations were found");
        }
        public IEnumerable<BusStation> GetAllBusStations()
        {
            var ListBS = (from BusStation bs in DataSource.AllBusStations
                          where bs.IsExist
                          select bs.Clone());
            if (ListBS != null)
                return ListBS;
            throw new DOException("No bus stations were found");
        }
        public int AddBusStation(BusStation bs ) 
        {
            bs.StationCode = ConfigurationClass.StationCode;
            DataSource.AllBusStations.Add(bs.Clone());
            return bs.StationCode;
        }
        public void DeleteBusStation(int _StationCode) //delete line-stations
        {
            int n = DataSource.AllBusStations.FindIndex(x => x.IsExist && x.StationCode == _StationCode);
            if(n>-1)
                DataSource.AllBusStations[n].IsExist = false;
            else
                throw new DOException($"Station number {_StationCode} was not found");
        }
        public IEnumerable<BusStation> GetStationsOfLine(int _LineCode)
        {
            IEnumerable<LineStation> lsLst = DataSource.AllLineStations.FindAll(x => x.IsExist && x.LineCode == _LineCode);
            IEnumerable<BusStation> bsLst = (from ls in lsLst
                                             orderby ls.StationNumberInLine
                                             select GetBusStation(ls.StationCode).Clone());
            if (bsLst != null)
                return bsLst;
            throw new DOException("No line stations were found");

        }
        #endregion

        #region Line
        public Line GetLine(int _Code)
        {
            Line l = DataSource.AllLines.Find(x => x.IsExist && x.Code == _Code);
            if(l!=null)
                return l.Clone();
            throw new DOException($"Line number {_Code} was not found");
        }
        public void UpdateLine(Line l) 
        {
            int n = DataSource.AllLines.FindIndex(x => x.Code == l.Code);
            DataSource.AllLines[n] = l.Clone();
    }
        public IEnumerable<Line> GetStationLines(int _StationCode) // all the lines which cross in this station
        {
           return from ls in DataSource.AllLineStations.FindAll(x => x.IsExist && x.StationCode == _StationCode)
                  where GetLine(ls.LineCode).IsExist
                  select GetLine(ls.LineCode);
        }

        public IEnumerable<Line> GetAllLines() 
        {
            var Listl = (from Line l in DataSource.AllLines
                         where l.IsExist
                         select l.Clone());
            if (Listl != null)
                return Listl;
            throw new DOException("No lines were found");
        }
        public IEnumerable<Line> GetSpecificLines(Predicate<Line> p)
        {
            var Listl = (from Line l in DataSource.AllLines
                        where p(l)
                        select l.Clone());
            if (Listl != null)
                return Listl;
            throw new DOException("No exist lines were found");
        }
        public int AddLine( Line l ) 
        {
            l.Code = ConfigurationClass.LineCode;
            DataSource.AllLines.Add(l.Clone());
            return l.Code;
        }

        public void DeleteLine(int _Code) 
        {
            int n = DataSource.AllLines.FindIndex(x => x.IsExist && x.Code == _Code);
            if(n>-1)
                DataSource.AllLines[n].IsExist = false;
            else
                throw new DOException($"Line number {_Code} was not found");
        }
        #endregion

        #region Line Station
        public void AddLineStation(int _LineCode, int _StationCode, int _StationNumberInLine) 
        {
            LineStation ls = new LineStation();
            ls.IsExist = true;
            ls.StationCode = _StationCode;
            ls.LineCode = _LineCode;
            ls.StationNumberInLine = _StationNumberInLine;
            DataSource.AllLineStations.Add(ls);
        }
        public LineStation GetLineStation(int _LineCode, int _StationCode) 
        {
            LineStation ls = DataSource.AllLineStations.Find(x => x.IsExist && (x.LineCode == _LineCode && x.StationCode == _StationCode));
            if (ls != null)
                return ls.Clone();
            throw new DOException($"Line number {_LineCode} does not cross in station {_StationCode}");
        }
        public IEnumerable<LineStation> GetAllLineStations(int _LineCode)
        {
            return DataSource.AllLineStations.FindAll(x => x.IsExist && x.LineCode == _LineCode);
        }
        public void UpdateLineStation(int _LineCode, int _StationCode,int n)//change index in +/-1
        {
            LineStation ls = GetLineStation(_LineCode, _StationCode);
           
            int ind=DataSource.AllLineStations.FindIndex(x => x.LineCode == _LineCode && x.StationCode == _StationCode);
            DataSource.AllLineStations[ind].StationNumberInLine += n;
        }
        public int IsStationInLine(int _LineCode, int _StationCode) //check if exist specific line station and return the station location in the line or -1
        {
            if (! DataSource.AllLineStations.Exists(x => x.IsExist && x.LineCode == _LineCode && x.StationCode == _StationCode))
                return -1;
            LineStation ls = GetLineStation(_LineCode, _StationCode);
            return ls.StationNumberInLine;
        }
        public IEnumerable<LineStation> GetSpecificLineStations(Predicate<LineStation> p)
        {
            IEnumerable<LineStation> Listl = (from LineStation l in DataSource.AllLineStations
                         where l.IsExist && p(l)
                         select l.Clone());
            return Listl;
            //return spesific collection OR NULL
        }

        public void DeleteLineStation(int _LineCode, int _StationCode)
        {
            int n = DataSource.AllLineStations.FindIndex(x => x.StationCode == _StationCode && x.LineCode == _LineCode);
            if (n > -1)
                DataSource.AllLineStations[n].IsExist = false;
            else
                throw new DOException($"Line number {_LineCode} does not cross in station {_StationCode}");

        }
        #endregion

        #region Consecutive Stations
       
        public void AddConsecutiveStations(ConsecutiveStations cs)
        {
            if(! isExistConsecutiveStations(cs.StationCode1,cs.StationCode2))
                DataSource.AllConsecutiveStations.ToList().Add(cs);
        }
        public ConsecutiveStations GetConsecutiveStations(int _StationCode1, int _StationCode2)
        {
            ConsecutiveStations cs = DataSource.AllConsecutiveStations.ToList().Find(x => x.StationCode1 == _StationCode1 && x.StationCode2 == _StationCode2);
            if (cs != null)
                return cs.Clone();
            throw new DOException($"Station {_StationCode1} and station {_StationCode2} are not consecutive stations");
        }

        public void UpdateConsecutiveStations(ConsecutiveStations cs)
        {
            int n = DataSource.AllConsecutiveStations.ToList().FindIndex(x => x.StationCode1 == cs.StationCode1 && x.StationCode2 == cs.StationCode2);
            if (n > -1)
            {
                var lst = DataSource.AllConsecutiveStations.ToList();
                lst.RemoveAt(n);
                lst.Add(cs.Clone());
                DataSource.AllConsecutiveStations = lst;
            }
            else
                throw new DOException($"Station {cs.StationCode1} and station {cs.StationCode2} are not consecutive stations");
        }

        public bool isExistConsecutiveStations(int _FirstStation, int _LastStation)
        {
            return DataSource.AllConsecutiveStations.ToList().Exists(x => x.StationCode1 == _FirstStation && x.StationCode2 == _LastStation);
        }

       public IEnumerable<ConsecutiveStations> GetSomeConsecutiveStations(int _StationCode)
        {
            return from ConsecutiveStations cs in DataSource.AllConsecutiveStations
                   where cs.StationCode1 == _StationCode || cs.StationCode2 == _StationCode
                   select cs;
        }
        public void DeleteConsecutiveStations(int _StationCode1, int _StationCode2)
        {
            List<ConsecutiveStations> lst = DataSource.AllConsecutiveStations.ToList();
            int n = lst.FindIndex(x => x.StationCode1 == _StationCode1 && x.StationCode2 == _StationCode2);
            if (n > -1)
            {
                lst.RemoveAt(n);
                DataSource.AllConsecutiveStations = lst;
            }
            else
                throw new DOException($"Station {_StationCode1} and station {_StationCode2} are not consecutive");
        }
            #endregion

            #region User
        public User GetUser(string name, string password)
        {
            User u = DataSource.AllUsers.FirstOrDefault(x => x.IsExist && x.Name == name && x.Password == password);
            if (u != null)
                return u.Clone();
            throw new DOException($"User name or password are wrong");
        }

        public void UpdateUser(User u)
        {
            int index = DataSource.AllUsers.FindIndex(x => x.IsExist && x.Name == u.Name);
            if (index > -1)
                DataSource.AllUsers[index] = u.Clone();
            throw new DOException($"User named {u.Name} was not found");
        }

        public IEnumerable<User> GetSpecificUsers(Predicate<User> p)
        {
            return from User u in DataSource.AllUsers
                   where p(u)
                   select u;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return from User u in DataSource.AllUsers
                   where u.IsExist
                   select u;
        }

        public void AddUser(User u)
        {
            if (!DataSource.AllUsers.Exists(x => x.IsExist && x.Name == u.Name))
                DataSource.AllUsers.Add(u.Clone());
            else
                throw new DOException($"User named {u.Name} is already exist");
        }

        public void DeleteUser(string name)
        {
            int index = DataSource.AllUsers.FindIndex(x => x.IsExist && x.Name == name);
            if (index > -1)
                DataSource.AllUsers.RemoveAt(index);
            throw new DOException($"User named {name} was not found");
        }
        #endregion

        #region LineTrip
        public LineTrip GetLineTrip(int _LineCode, TimeSpan _Start)
        {
            LineTrip lt = DataSource.AllLinesTrip.Find(x =>x.IsExist && x.LineCode == _LineCode && x.Start <= _Start);
            if (lt != null)
                return lt.Clone();
            throw new DOException($"Line {_LineCode} has no trip till {_Start}");
        }

        public void AddLineTrip(LineTrip lt)
        {
            //can add identical linetrip, rush hours..
            DataSource.AllLinesTrip.Add(lt.Clone());
        }

        public IEnumerable<LineTrip> GetAllLineTrips(int _LineCode)
        {
            return from LineTrip x in DataSource.AllLinesTrip
                   where x.LineCode == _LineCode && x.IsExist
                   select x.Clone();
        }

        public IEnumerable<LineTrip> GetAllStationLineTrips(int _StationCode, TimeSpan _Start)
        {
            List<LineStation> ls = DataSource.AllLineStations.FindAll(x => x.IsExist && x.StationCode == _StationCode);
            if (ls == null)
                throw new DOException($"Bus station {_StationCode} has no lines");

             IEnumerable<LineTrip> lt = from LineStation t in ls
                                        from LineTrip x in DataSource.AllLinesTrip
                                        where x.LineCode == t.LineCode && x.Start <= _Start && x.IsExist
                                        select x.Clone();
            return lt;
        }

        public void DeleteLineTrip(int _LineCode, TimeSpan _Start)
        {
            int n = DataSource.AllLinesTrip.FindIndex(x => x.IsExist&& x.LineCode == _LineCode &&x.Start==_Start);
            if (n > -1)
                DataSource.AllLinesTrip[n].IsExist = false; 
            else
                throw new DOException($"Line {_LineCode} has no trip at {_Start}");
        }
        public void UpdateLineTrip(LineTrip lt, TimeSpan NewStart) //lt==original
        {
            int n = DataSource.AllLinesTrip.FindIndex(x => x.IsExist &&  x.LineCode == lt.LineCode && x.Start == lt.Start);
            if (n > -1)
            {
                lt.Start = NewStart;
                DataSource.AllLinesTrip[n] = lt;
            }
            else
                throw new DOException($"Line {lt.LineCode} has no trip at {lt.Start}");
        }
        #endregion
    }
}