using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace Terminal
{
    static class Program
    {
        [MTAThread]
        static void Main()
        {
            Cache cache = new Cache();
            try
            {
                cache.LoadCache();
            }
            catch
            {
                MessageBox.Show("Ошибка инициализации, файл кэша поврежден или отсутствует",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            LogonForm logonForm = new LogonForm(cache);
            Cursor.Current = Cursors.Default;
            Application.Run(logonForm);
            if (logonForm.DialogResult == DialogResult.Cancel) return;

            Cursor.Current = Cursors.WaitCursor;
            MainForm mainForm = new MainForm(cache);
            Cursor.Current = Cursors.Default;
            Application.Run(mainForm);
        }
    }

    public struct ProdsListRecord
    {
        public string prodId;
        public string timeDateLine;
        public string defId;
        public string deffect;
        public string contId;
        public string controller;
        public int defsCount;

        public ProdsListRecord(string pId, string tmDtLn, string dId, string def,
            string cId, string cont, int dCount)
        {
            prodId = pId;
            timeDateLine = tmDtLn;
            defId = dId;
            deffect = def;
            contId = cId;
            controller = cont;
            defsCount = dCount;
        }
    }
    public struct RegsListRecord
    {
        public string time;
        public string defInfo;
        public string date;
        public string contInfo;
        public RegsListRecord(string tm, string dInfo, string dt, string cInfo)
        {
            time = tm;
            defInfo = dInfo;
            date = dt;
            contInfo = cInfo;
        }
    }
    public struct DefsListRecord
    {
        public string id;
        public string name;
        public DefsListRecord(string dId, string dName)
        {
            id = dId;
            name = dName;
        }
    }
    public struct ContsListRecord
    {
        public string id;
        public string name;
        public ContsListRecord(string cId, string cName)
        {
            id = cId;
            name = cName;
        }
    }
    public class ProgSettings
    {
        public static int line = 1;
        public static TimeSpan period = new TimeSpan(0, 15, 0);
        public static int maxProdsListLength = 10;
        public static int maxProdsCacheLength = 100;
        public static bool permanentCont;
        public static string permContId;
        public static int termId = 1;
        public static string servIp;
        public static string termIp;
    }
    public enum RegState
    {
        Unchanged, New, NewTemp, Nonexistent
    }

    public class Cache
    {
        ArrayList products;
        ArrayList deffects;
        ArrayList workers;
        ArrayList tempRegs;

        enum WorkerType
        {
            Cont, Oper
        }
        class Deffect : IComparable, ICloneable
        {
            public string id;
            public string name;
            public Deffect(string dId, string dName)
            {
                id = dId;
                name = dName;
            }
            public Deffect(string dId)
            {
                id = dId;
            }
            public int CompareTo(object o)
            {
                return id.CompareTo((o as Deffect).id);
            }
            public object Clone()
            {
                return new Deffect(id, name);
            }
        }
        class Worker : IComparable, ICloneable
        {
            public string id;
            public string name;
            public WorkerType type;
            public string password;
            public Worker(string wId, string wName, string wPsw, WorkerType wType)
            {
                id = wId;
                name = wName;
                password = wPsw;
                type = wType;
            }
            public Worker(string workId)
            {
                id = workId;
            }
            public int CompareTo(object o)
            {
                return id.CompareTo((o as Worker).id);
            }
            public object Clone()
            {
                return new Worker(id, name, password, type);
            }
        }
        class Registration : IComparable, ICloneable
        {
            public DateTime time;
            public int line;
            public string defId;
            public string contId;
            public RegState state;
            public Registration(DateTime rTime, int rLine,
                string dId, string cId, RegState rState)
            {
                time = rTime;
                line = rLine;
                defId = dId;
                contId = cId;
                state = rState;
            }
            public int CompareTo(object o)
            {
                return time.CompareTo((o as Registration).time);
            }
            public object Clone()
            {
                return new Registration(time, line, defId, contId, state);
            }
        }
        class Product : ICloneable
        {
            public string id;
            public ArrayList regs;
            public Product(string pId, ArrayList pRegs)
            {
                id = pId;
                regs = pRegs;
            }
            public Product(string pId)
            {
                id = pId;
            }
            public object Clone()
            {
                if (regs == null) return new Product(id, null);
                ArrayList arrLstRegs = new ArrayList();
                foreach (Registration r in regs)
                    arrLstRegs.Add(r.Clone());
                return new Product(id, arrLstRegs);
            }
        }

        public Cache()
        {
            products = new ArrayList();
            deffects = new ArrayList();
            workers = new ArrayList();
            tempRegs = new ArrayList();
        }

        #region Взаимодействие с ССД
        public void UpdateCatalogs()
        {
            deffects = new ArrayList();
            workers = new ArrayList();

            deffects.Add(new Deffect("460678200168", "Царапина"));
            deffects.Add(new Deffect("460192120232", "Нет ручки"));
            deffects.Add(new Deffect("978591180369", "Нет двери"));
            workers.Add(new Worker("460678200168", "Иванов И.И.", "460678200168", WorkerType.Cont));
            workers.Add(new Worker("460192120232", "Петров П.П.", "222", WorkerType.Cont));
            workers.Add(new Worker("978591180369", "Сидоров П.П.", "333", WorkerType.Cont));
            workers.Add(new Worker("1", "Степанов Л.Л.", "1", WorkerType.Oper));
            deffects.Sort();
            workers.Sort();
            deffects.TrimToSize();
            workers.TrimToSize();
        }
        public void ReplicateRegs()
        {
        }
        #endregion

        #region Работа с временным списком регистраций
        public void FillTempRegs(string prodId)
        {
            tempRegs.Clear();
            Product p = GetProd(prodId);
            if (p == null || p.regs == null) return;
            foreach (Registration r in p.regs)
                if (DateTime.Now < r.time.Add(ProgSettings.period))
                    tempRegs.Add(r.Clone());
            tempRegs.Sort();
        }
        public void AddTempReg(string defId, string contId)
        {
            tempRegs.Add(new Registration(DateTime.Now, ProgSettings.line,
                defId, contId, RegState.NewTemp));
        }
        public void DelTempReg(string defId)
        {
            for (int i = 0; i < tempRegs.Count; i++)
            {
                Registration r = tempRegs[i] as Registration;
                if (r.defId == defId)
                {
                    tempRegs.RemoveAt(i);
                    return;
                }
            }
        }
        public RegState GetTempRegState(string defId)
        {
            foreach (Registration r in tempRegs)
                if (r.defId == defId)
                    return r.state;
            return RegState.Nonexistent;
        }
        public bool IsTempRegsListChanged()
        {
            foreach (Registration r in tempRegs)
                if (r.state == RegState.NewTemp)
                    return true;
            return false;
        }
        public void AcceptTempRegs(string prodId)
        {
            Product p = GetProd(prodId);
            if (p == null)
            {
                if (products.Count >= ProgSettings.maxProdsCacheLength)
                {
                    foreach (Registration r in (products[0] as Product).regs)
                        if (r.state != RegState.Unchanged)
                            throw new Exception("Кэш переполнен");
                    products.RemoveAt(0);
                }
                p = new Product(prodId, new ArrayList());
                products.Add(p);
            }
            foreach (Registration r in tempRegs)
                if (r.state == RegState.NewTemp)
                {
                    p.regs.Add(new Registration(r.time, r.line,
                        r.defId, r.contId, RegState.New));
                }
            p.regs.Sort();
            products.Remove(p);
            products.Add(p);
        }        
        #endregion

        #region Формирование данных для списков
        public ProdsListRecord[] GetProdsListContents()
        {
            ArrayList res = new ArrayList();
            string tm, dt, ln;
            if (products.Count == 0) return null;
            for (int i = 0; i < ProgSettings.maxProdsListLength &&
                products.Count - i > 0; i++)
            {

                Product p = products[products.Count - 1 - i] as Product;
                Registration r = p.regs[p.regs.Count - 1] as Registration;
                ProdsListRecord rec = new ProdsListRecord();
                tm = r.time.Hour.ToString().PadLeft(2, '0') + ":" +
                    r.time.Minute.ToString().PadLeft(2, '0');
                dt = r.time.Day.ToString().PadLeft(2, '0') + "." +
                    r.time.Month.ToString().PadLeft(2, '0');
                ln = r.line.ToString();
                rec.prodId = p.id;
                rec.timeDateLine = tm + "|" + dt + "|Л" + ln;
                rec.defId = r.defId;
                rec.deffect = GetDef(r.defId).name;
                rec.contId = r.contId;
                rec.controller = GetCont(r.contId).name;
                int dCount = 0;
                foreach (Registration reg in p.regs)
                    if (DateTime.Now < reg.time.Add(ProgSettings.period))
                        dCount++;
                rec.defsCount = dCount;
                res.Add(rec);
            }
            res.Reverse();
            return (ProdsListRecord[])res.ToArray(typeof(ProdsListRecord));
        }
        public RegsListRecord[] GetRegsListContents()
        {
            ArrayList res = new ArrayList();
            foreach (Registration r in tempRegs)
            {
                RegsListRecord rec = new RegsListRecord();
                rec.time = r.time.Hour.ToString().PadLeft(2, '0') + ":" +
                    r.time.Minute.ToString().PadLeft(2, '0');
                rec.date = r.time.Day.ToString().PadLeft(2, '0') + "." +
                    r.time.Month.ToString().PadLeft(2, '0');
                rec.defInfo = r.defId + " (" + GetDef(r.defId).name + ")";
                rec.contInfo = r.contId + " (" + GetCont(r.contId).name + ")";
                res.Add(rec);
            }
            return (RegsListRecord[])res.ToArray(typeof(RegsListRecord));
        }
        public DefsListRecord[] GetDefsListContents()
        {
            ArrayList res = new ArrayList();
            foreach (Deffect d in deffects)
                res.Add(new DefsListRecord(d.id, d.name));
            return (DefsListRecord[])res.ToArray(typeof(DefsListRecord));
        }
        public ContsListRecord[] GetContsListContents()
        {
            ArrayList res = new ArrayList();
            foreach (Worker w in workers)
                if (w.type == WorkerType.Cont)
                    res.Add(new ContsListRecord(w.id, w.name));
            return (ContsListRecord[])res.ToArray(typeof(ContsListRecord));
        }
        #endregion

        #region Проверка правильности кодов
        public bool VerifyDefId(string defId)
        {
            int index = deffects.BinarySearch(0, deffects.Count,
                new Deffect(defId), null);
            if (index >= 0) return true;
            return false;
        }
        public bool VerifyContId(string contId)
        {
            int index = workers.BinarySearch(0, workers.Count,
                new Worker(contId), null);
            if (index < 0) return false;
            Worker w = workers[index] as Worker;
            if (w.type == WorkerType.Cont) return true;
            return false;
        }
        public bool VerifyOperPassword(string wId, string psw)
        {
            int index = workers.BinarySearch(0, workers.Count,
                new Worker(wId), null);
            if (index < 0) return false;
            Worker w = workers[index] as Worker;
            if (w.type == WorkerType.Cont || w.password != psw)
                return false;
            return true;
        }
        #endregion

        #region Вспомогательные процедуры
        Deffect GetDef(string id)
        {
            int index = deffects.BinarySearch(0, deffects.Count,
                new Deffect(id), null);
            if (index < 0)
                throw new Exception("Неверный код деффекта");
            return deffects[index] as Deffect;
        }
        Worker GetCont(string id)
        {
            int index = workers.BinarySearch(0, workers.Count,
                new Worker(id), null);
            if (index < 0)
                throw new Exception("Неверный код контроллера");
            return workers[index] as Worker;
        }
        Product GetProd(string id)
        {
            foreach(Product p in products)
                if(p.id == id) return p;
            return null;
        }
        public void SaveCache()
        {
            FileStream stream = new FileStream("cache.rwk",
                FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream,
                System.Text.Encoding.Unicode);
            writer.Write(products.Count);
            foreach (Product p in products)
            {
                writer.Write(p.id);
                writer.Write(p.regs.Count);
                foreach (Registration r in p.regs)
                {
                    writer.Write(r.time.ToFileTime());
                    writer.Write(r.line);
                    writer.Write(r.defId);
                    writer.Write(r.contId);
                    writer.Write(r.state.ToString());
                }
            }

            writer.Write(deffects.Count);
            foreach (Deffect d in deffects)
            {
                writer.Write(d.id);
                writer.Write(d.name);
            }

            writer.Write(workers.Count);
            foreach (Worker w in workers)
            {
                writer.Write(w.id);
                writer.Write(w.name);
                writer.Write(w.type.ToString());
                writer.Write(w.password);
            }
            writer.Close();
        }
        public void LoadCache()
        {
            FileStream stream = new FileStream("cache.rwk",
                FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream,
                System.Text.Encoding.Unicode);
            int pCount = reader.ReadInt32();
            products = new ArrayList(pCount);
            for (int i = 0; i < pCount; i++)
            {
                string pId = reader.ReadString();
                int rCount = reader.ReadInt32();
                ArrayList regs = new ArrayList(rCount);
                for (int j = 0; j < rCount; j++)
                {
                    DateTime time = DateTime.FromFileTime(reader.ReadInt64());
                    int line = reader.ReadInt32();
                    string dId = reader.ReadString();
                    string cId = reader.ReadString();
                    RegState state = (RegState)
                        Enum.Parse(typeof(RegState), reader.ReadString(), true);
                    regs.Add(new Registration(time, line, dId, cId, state));
                }
                products.Add(new Product(pId, regs));
            }

            int dCount = reader.ReadInt32();
            deffects = new ArrayList(dCount);
            for (int i = 0; i < dCount; i++)
            {
                string dId = reader.ReadString();
                string dName = reader.ReadString();
                deffects.Add(new Deffect(dId, dName));
            }

            int wCount = reader.ReadInt32();
            workers = new ArrayList(wCount);
            for (int i = 0; i < wCount; i++)
            {
                string wId = reader.ReadString();
                string wName = reader.ReadString();
                WorkerType type = (WorkerType)
                    Enum.Parse(typeof(WorkerType), reader.ReadString(), true);
                string password = reader.ReadString();
                workers.Add(new Worker(wId, wName, password, type));
            }
            reader.Close();
        }
        #endregion
    }
}