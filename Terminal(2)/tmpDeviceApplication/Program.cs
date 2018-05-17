using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;

namespace Terminal
{
    static class Program
    {
        [MTAThread]
        static void Main()
        {
            Application.Run(new MainFormDeffectsOnly());
        }
    }

    /// <summary>
    /// Состояние записи о дефекте
    /// </summary>
    public enum RegState
    {
        Unchanged, New, DeletedTemp, NewTemp
    }
    
    /// <summary>
    /// Вид дефекта
    /// </summary>
    public class Deffect : IComparable, ICloneable
    {
        public string ID;
        public string Name;

        public Deffect(string id, string name)
        {
            ID = id;
            Name = name;
        }
        public Deffect(string id)
        {
            ID = id;
        }
        public int CompareTo(object d)
        {
            return ID.CompareTo((d as Deffect).ID);
        }
        public object Clone()
        {
            return new Deffect(ID, Name);
        }
    }

    /// <summary>
    /// Оператор
    /// </summary>
    public class Worker : IComparable, ICloneable
    {
        public string ID;
        public string Name;
        public string Password;

        public Worker(string id, string name, string password)
        {
            ID = id;
            Name = name;
            Password = password;
        }
        public Worker(string id)
        {
            ID = id;
        }
        public int CompareTo(object w)
        {
            return ID.CompareTo((w as Worker).ID);
        }
        public object Clone()
        {
            return new Worker(ID, Name, Password);
        }
    }

    /// <summary>
    /// Запись о деффекте
    /// </summary>
    public class Registration : IComparable, ICloneable
    {
        public DateTime Time;
        public int Line;
        public string DefID;
        public string WorkID;
        public RegState State;

        public Registration(DateTime time, int line, string dID, string wID, RegState state)
        {
            Time = time;
            Line = line;
            DefID = dID;
            WorkID = wID;
            State = state;
        }
        public int CompareTo(object r)
        {
            return Time.CompareTo((r as Registration).Time);
        }
        public object Clone()
        {
            return new Registration(Time, Line, DefID, WorkID, State);
        }
    }

    /// <summary>
    /// Изделие
    /// </summary>
    public class Product : IComparable, ICloneable
    {
        public string ID;
        public string ModID;
        public string Model;
        public ArrayList Regs;

        public Product(string id, string mID, string model, ArrayList regs)
        {
            ID = id;
            ModID = mID;
            Model = model;
            Regs = regs;
        }
        public Product(string id)
        {
            ID = id;
        }
        public int CompareTo(object p)
        {
            return ID.CompareTo((p as Product).ID);
        }
        public object Clone()
        {
            ArrayList regs = null;
            if (Regs != null)
            {
                regs = new ArrayList();
                foreach (Registration r in Regs)
                    regs.Add(r.Clone());
            }
            return new Product(ID, ModID, Model, regs);
        }
    }

    /// <summary>
    /// Кэш
    /// </summary>
    public class Cache
    {
        private ArrayList Products;
        private ArrayList Deffects;
        private ArrayList Workers;
        private ArrayList TempRegs;

        public string SelectedProdID;
        public string SelectedDefID;
        public string SelectedWorkID;
        public int Line;

        public Cache() { }

        #region Взаимодействие с БД
        /// <summary>
        /// Обновление справочников
        /// </summary>
        public void UpdateCatalogs()
        {
            Deffects = new ArrayList();
            Workers = new ArrayList();

            Deffects.Add(new Deffect("R44-2695", "(Инстр) Нет чего-то"));
            Deffects.Add(new Deffect("460192120232", "(Тетр) Нет того-то"));
            Deffects.Add(new Deffect("3", "Царапина"));
            Deffects.Add(new Deffect("4", "Вмятина"));
            Deffects.Add(new Deffect("5", "Непрокрашено"));
            Deffects.Add(new Deffect("6", "Разбита лампочка"));
            Workers.Add(new Worker("R44-2695", "Иванов И.И.", "111"));
            Workers.Add(new Worker("460192120232", "Петров П.П.", "222"));
            Workers.Add(new Worker("3", "Сидоров П.П.", "333"));
            Workers.Add(new Worker("4", "Степанов Л.Л.", "444"));
            Deffects.Sort();
            Workers.Sort();
        }

        /// <summary>
        /// Получение информации об изделии из БД
        /// </summary>
        public void UpdateProdInfo()
        {
            int index = Prods.BinarySearch(0, Prods.Count, new Product(SelectedProdID), null);
            if (index < 0) throw new Exception("Неверный код изделия");
            Product p = Prods[index] as Product;
            if (p.Regs == null)
            {
                Products.Add(p.Clone());
                return;
            }
            ArrayList regs = null;
            foreach (Registration r in p.Regs)
                if (DateTime.Now < r.Time.Add(ProgramSettings.ReentrancePeriod))
                {
                    if (regs == null) regs = new ArrayList();
                    regs.Add(r.Clone());
                }
            if (regs != null)
            {
                regs.Sort();
                regs.Reverse();
            }
            p = new Product(p.ID, p.ModID, p.Model, regs);
            Products.Add(p);
        }

        /// <summary>
        /// Репликация всех регистраций
        /// </summary>
        private void ReplicateAllRegs()
        {
        }

        /// <summary>
        /// Репликация последних регистраций
        /// </summary>
        private void ReplicateLastRegs()
        {
        }
        #endregion

        #region Работа со списком временных регистраций
        /// <summary>
        /// Принятие временных регистраций
        /// </summary>
        public void AcceptTempRegs()
        {
            Product p = GetProd(SelectedProdID);
            foreach (Registration r in TempRegs)
                if (r.State == RegState.NewTemp)
                {
                    Registration nr = new Registration(r.Time,
                        r.Line, r.DefID, r.WorkID, RegState.New);
                    if (p.Regs == null) p.Regs = new ArrayList();
                    p.Regs.Add(nr);
                }
            p.Regs.Sort();
            p.Regs.Reverse();
        }

        /// <summary>
        /// Проверка, не зарегистрирован ли уже данный дефект
        /// </summary>
        public bool DefAlreadyRegistered()
        {
            foreach (Registration r in TempRegs)
                if (r.DefID == SelectedDefID && r.State != RegState.DeletedTemp)
                    return true;
            return false;
        }
        
        /// <summary>
        /// Добавление записи о дефекте в список временных регистраций
        /// </summary>
        public void AddTempReg()
        {
            TempRegs.Add(new Registration(DateTime.Now, Line,
                SelectedDefID, SelectedWorkID, RegState.NewTemp));
        }

        /// <summary>
        /// Удаление записи о дефекте из списка временных регистраций
        /// </summary>
        public void DelTempReg()
        {
            foreach (Registration r in TempRegs)
                if (r.DefID == SelectedDefID)
                {
                    r.State = RegState.DeletedTemp;
                    return;
                }
        }

        /// <summary>
        /// Заполнение списка временных регистраций
        /// </summary>
        public void FillTempRegs()
        {
            Product p = GetProd(SelectedProdID);
            TempRegs.Clear();
            if (p.Regs == null) return;
            foreach (Registration r in p.Regs)
                if (r.Time.Add(ProgramSettings.ReentrancePeriod) > DateTime.Now)
                    TempRegs.Add(r.Clone());
            TempRegs.Sort();
        }

        /// <summary>
        /// Получение состояния записи о дефекте
        /// </summary>
        public string GetTempRegState()
        {
            foreach(Registration r in TempRegs)
                if (r.DefID == SelectedDefID && r.State != RegState.DeletedTemp)
                    return r.State.ToString();
            throw new Exception("Запись о дефекте не найдена");
        }

        /// <summary>
        /// Проверка, были ли сделаны изменения в списке временных регистраций
        /// </summary>
        public bool TempRegsAdded()
        {
            foreach (Registration r in TempRegs)
                if (r.State == RegState.NewTemp)
                    return true;
            return false;
        }
        #endregion

        #region Подготовка данных для списков
        /// <summary>
        /// Получение содержимого списка изделий
        /// </summary>
        public ListViewItem[] GetProdsListContents()
        {
            ArrayList res = new ArrayList();
            string time, date, line, dID, def, wID, work;
            for (int i = 0; i < Products.Count; i++)
            {
                Product p = Products[i] as Product;
                if (p.Regs == null || p.Regs.Count == 0) continue;
                Registration r = p.Regs[0] as Registration;
                time = r.Time.ToShortTimeString();
                date = r.Time.ToShortDateString();
                line = r.Line.ToString();
                dID = r.DefID;
                def = GetDef(dID).Name;
                wID = r.WorkID;
                work = GetWork(wID).Name;
                res.Add(new ListViewItem(new string[] { time, line, "Код", p.ID }));
                res.Add(new ListViewItem(new string[] { date, "", "Модель", p.ModID + " (" + p.Model + ")" }));
                res.Add(new ListViewItem(new string[] { "", "", "Дефект", dID + " (" + def + ")" }));
                res.Add(new ListViewItem(new string[] { "", "", "Оператор", wID + " (" + work + ")" }));
                if (i != Products.Count - 1) res.Add(new ListViewItem(new string[] { "", "", "", "" }));
            }
            return (ListViewItem[])res.ToArray(typeof(ListViewItem));
        }

        /// <summary>
        /// Получение содержимого списка регистраций
        /// </summary>
        public ListViewItem[] GetRegsListContents()
        {
            ArrayList res = new ArrayList();
            string time, state = "", dID, def, wID, work;
            foreach (Registration r in TempRegs)
            {
                time = r.Time.ToShortTimeString();
                dID = r.DefID;
                def = GetDef(dID).Name;
                wID = r.WorkID;
                work = GetWork(wID).Name;
                if (r.State == RegState.New) state = "N";
                if (r.State == RegState.DeletedTemp) state = "D";
                if (r.State == RegState.Unchanged) state = "U";
                res.Add(new ListViewItem(new string[]
                    { state + " " + time, dID + " (" + def + ")", wID + " (" + work + ")" }));
            }
            return (ListViewItem[])res.ToArray(typeof(ListViewItem));
        }

        /// <summary>
        /// Получение содержимого списка дефектов
        /// </summary>
        public ListViewItem[] GetDefsListContents()
        {
            ArrayList res = new ArrayList();
            foreach (Deffect d in Deffects)
                res.Add(new ListViewItem(new string[] { d.ID, d.Name }));
            return (ListViewItem[])res.ToArray(typeof(ListViewItem));
        }

        /// <summary>
        /// Получение содержимого списка операторов
        /// </summary>
        public ListViewItem[] GetWorksListContents()
        {
            ArrayList res = new ArrayList();
            foreach (Worker w in Workers)
                res.Add(new ListViewItem(new string[] { w.ID, w.Name }));
            return (ListViewItem[])res.ToArray(typeof(ListViewItem));
        }

        /// <summary>
        /// Получение записи списка изделий
        /// </summary>
        public ListViewItem[] GetProdsListRecord()
        {
            ArrayList res = new ArrayList();
            string time, date, line, dID, def, wID, work;
            Product p = GetProd(SelectedProdID);
            Registration r = p.Regs[0] as Registration;
            time = r.Time.ToShortTimeString();
            date = r.Time.ToShortDateString();
            line = r.Line.ToString();
            dID = r.DefID;
            def = GetDef(dID).Name;
            wID = r.WorkID;
            work = GetWork(wID).Name;
            res.Add(new ListViewItem(new string[] { time, line, "Код", p.ID }));
            res.Add(new ListViewItem(new string[] { date, "", "Модель", p.ModID + " (" + p.Model + ")" }));
            res.Add(new ListViewItem(new string[] { "", "", "Дефект", dID + " (" + def + ")" }));
            res.Add(new ListViewItem(new string[] { "", "", "Оператор", wID + " (" + work + ")" }));
            return (ListViewItem[])res.ToArray(typeof(ListViewItem));
        }

        /// <summary>
        /// Получение записи списка регистраций
        /// </summary>
        public ListViewItem GetRegsListRecord()
        {
            string time, state = "", def, work;
            ListViewItem res = null;
            foreach (Registration r in TempRegs)
            {
                if (r.DefID == SelectedDefID && r.State != RegState.DeletedTemp)
                {
                    time = r.Time.ToShortTimeString();
                    def = GetDef(SelectedDefID).Name;
                    work = GetWork(SelectedWorkID).Name;
                    if (r.State == RegState.New) state = "N";
                    if (r.State == RegState.NewTemp) state = "T";
                    if (r.State == RegState.Unchanged) state = "U";
                    res = new ListViewItem(new string[] { state + " " + time,
                        r.DefID + " (" + def + ")", r.WorkID + " (" + work + ")" });
                    break;
                }
            }
            return res;
        }
        #endregion

        #region Проверка правильности кодов (по кэшу)
        /// <summary>
        /// Проверка кода изделия
        /// </summary>
        public bool VerifyProdID()
        {
            int index = Products.BinarySearch(0, Products.Count, new Product(SelectedProdID), null);
            if(index >= 0) return true;
            return false;
        }

        /// <summary>
        /// Проверка кода дефекта
        /// </summary>
        public bool VerifyDefID()
        {
            int index = Deffects.BinarySearch(0, Deffects.Count, new Deffect(SelectedDefID), null);
            if (index >= 0) return true;
            return false;
        }

        /// <summary>
        /// Проверка кода оператора
        /// </summary>
        public bool VerifyWorkID()
        {
            int index = Workers.BinarySearch(0, Workers.Count, new Worker(SelectedWorkID), null);
            if (index >= 0) return true;
            return false;
        }

        /// <summary>
        /// Проверка правильности пароля
        /// </summary>
        public bool VerifyPassword(string wID, string passw)
        {
            SelectedWorkID = wID;
            if (VerifyWorkID() == false)
                return false;
            Worker w = GetWork(wID);
            if (w.Password != passw)
                return false;
            return true;
        }
        #endregion

        #region Вспомогательные процедуры
        /// <summary>
        /// Получение дефекта из кэша по коду
        /// </summary>
        private Deffect GetDef(string ID)
        {
            int index = Deffects.BinarySearch(0, Deffects.Count, new Deffect(ID), null);
            if (index < 0) throw new Exception("Нарушение целостности данных, неверный код деффекта");
            return Deffects[index] as Deffect;
        }

        /// <summary>
        /// Получение оператора из кэша по коду
        /// </summary>
        private Worker GetWork(string ID)
        {
            int index = Workers.BinarySearch(0, Workers.Count, new Worker(ID), null);
            if (index < 0) throw new Exception("Нарушение целостности данных, неверный код оператора");
            return Workers[index] as Worker;
        }

        /// <summary>
        /// Получение изделия из кэша по коду
        /// </summary>
        private Product GetProd(string ID)
        {
            int index = Products.BinarySearch(0, Products.Count, new Product(ID), null);
            if (index < 0) throw new Exception("Нарушение целостности данных, неверный код изделия");
            return Products[index] as Product;
        }
        #endregion

        #region Temporary
        /// <summary>
        /// Конструктор
        /// </summary>
        public Cache(string s)
        {
            Products = new ArrayList();

            Prods.Add(new Product("5555555555", "5", "Indesit555", null));
            Prods.Add(new Product("6666666666", "6", "Indesit666", null));
            Prods.Add(new Product("7777777777", "7", "Indesit777", null));

            ArrayList regs = new ArrayList();
            regs.Add(new Registration(new DateTime(2008, 4, 15, 15, 46, 0), 1, "460192120232", "460192120232", RegState.Unchanged));
            regs.Add(new Registration(new DateTime(2008, 4, 15, 15, 46, 1), 1, "460192120232", "460192120232", RegState.Unchanged));
            regs.Add(new Registration(new DateTime(2008, 4, 15, 15, 46, 2), 1, "3", "460192120232", RegState.Unchanged));
            Product p = new Product("R44-2695", "1", "Indesit111", regs);
            Prods.Add(p);
            Products.Add(p.Clone());

            regs = new ArrayList();
            regs.Add(new Registration(new DateTime(2008, 4, 1, 14, 6, 0), 1, "4", "460192120232", RegState.Unchanged));
            regs.Add(new Registration(new DateTime(2008, 4, 1, 14, 7, 1), 1, "5", "460192120232", RegState.Unchanged));
            regs.Add(new Registration(new DateTime(2008, 4, 1, 14, 8, 2), 1, "6", "3", RegState.Unchanged));
            p = new Product("460192120232", "1", "Indesit222", regs);
            Prods.Add(p);
            Products.Add(p.Clone());

            regs = new ArrayList();
            regs.Add(new Registration(new DateTime(2008, 4, 1, 15, 3, 0), 1, "460192120232", "460192120232", RegState.Unchanged));
            regs.Add(new Registration(new DateTime(2008, 4, 1, 15, 4, 1), 1, "3", "460192120232", RegState.Unchanged));
            regs.Add(new Registration(new DateTime(2008, 4, 1, 15, 5, 2), 1, "6", "460192120232", RegState.Unchanged));
            p = new Product("3333333333", "1", "Indesit333", regs);
            Prods.Add(p);
            Products.Add(p.Clone());

            regs = new ArrayList();
            regs.Add(new Registration(new DateTime(2008, 4, 1, 16, 3, 0), 1, "460192120232", "460192120232", RegState.Unchanged));
            regs.Add(new Registration(new DateTime(2008, 4, 1, 16, 5, 1), 1, "460192120232", "3", RegState.Unchanged));
            regs.Add(new Registration(new DateTime(2008, 4, 1, 16, 7, 2), 1, "5", "460192120232", RegState.Unchanged));
            regs.Add(new Registration(new DateTime(2008, 4, 1, 16, 7, 2), 1, "3", "460192120232", RegState.Unchanged));
            p = new Product("4444444444", "1", "Indesit444", regs);
            Prods.Add(p);
            Products.Add(p.Clone());
            
            Prods.Sort();
            Products.Sort();
            TempRegs = new ArrayList();
        }
        
        /// <summary>
        /// Массив для имитации работы с ССД
        /// </summary>
        private ArrayList Prods = new ArrayList();
        #endregion
    }
}