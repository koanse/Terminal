using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Terminal
{
    /// <summary> Главная форма для режима работы "вход-выход" </summary>
    public partial class MainFormDeffectsOnly : Form
    {
        #region Элементы управления
        ListView ProductsList;
        ListView RegistrationsList;
        ListView DeffectsList;
        ListView WorkersList;
        Button Apply;
        Button Add;
        Button Back;
        Button Change;
        Button Select;
        Button Cancel;
        StatusBar StatusBar;
        MainMenu MainMenu;
        MenuItem FileMenuItem;
        MenuItem ExitMenuItem;
        #endregion

        Cache Cache;
        ProgramState State;
        
        /// <summary> Действие пользователя </summary>
        private enum UserAction
        { Add, Select, Back, Change, Undefined }

        /// <summary> Положение кнопки </summary>
        private enum ButtonAlignment
        { Left, Right, Center }

        /// <summary> Состояние работы программы </summary>
        enum ProgramState
        { InitialState, ProductsList }

        #region Инициализация
        /// <summary> Конструктор главной формы </summary>
        public MainFormDeffectsOnly()
        {
            SuspendLayout();
            MainMenu = new MainMenu();
            FileMenuItem = new MenuItem();
            ExitMenuItem = new MenuItem();
            StatusBar = new StatusBar();
            InitializeForm();
            InitializeMenu();
            InitializeStatusBar();
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary> Создание необходимых списков и кнопок </summary>
        void MainForm_Load(object sender, EventArgs e)
        {
            MinimumSize = Size;
  
            string[] Captions = new string[] { "Дата", "Линия", "Атрибут", "Значение" };
            int[] Proportions = new int[] { 2, 2, 3, 4 };
            ProductsList = CreateListView(Captions, Proportions);
            Captions = new string[] { "Дата", "Дефект", "Оператор" };
            Proportions = new int[] { 2, 5, 5 };
            RegistrationsList = CreateListView(Captions, Proportions);
            Captions = new string[] { "Код", "Дефект" };
            Proportions = new int[] { 2, 5 };
            DeffectsList = CreateListView(Captions, Proportions);
            Captions = new string[] { "Код", "Оператор" };
            Proportions = new int[] { 2, 5 };
            WorkersList = CreateListView(Captions, Proportions);

            Select = CreateButton("Выбрать", ButtonAlignment.Center, 1);
            Back = CreateButton("Назад", ButtonAlignment.Right, 2);

            Cache = new Cache();
            State = ProgramState.InitialState;
            Manager(UserAction.Undefined);
        }

        /// <summary> Инициализация формы </summary>
        private void InitializeForm()
        {
            // Инициализация формы
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Menu = MainMenu;
            Name = "MainForm";
            Text = "ICS Reworks: сбор данных";
            WindowState = FormWindowState.Maximized;
            Load += new EventHandler(MainForm_Load);
        }

        /// <summary> Инициализация строки состояния </summary>
        private void InitializeStatusBar()
        {
            StatusBar.Location = new Point(0, ClientSize.Height - StatusBar.Height);
            StatusBar.Name = "StatusBar";
            StatusBar.Parent = this;
            StatusBar.SizingGrip = false;
            StatusBar.TabIndex = 6;
            StatusBar.Text = "Строка состояния";
        }

        /// <summary> Инициализация меню </summary>
        private void InitializeMenu()
        {
            MainMenu.MenuItems.Add(FileMenuItem);
            MainMenu.Name = "MainMenu";

            // Инициализация пункта меню "Файл"
            FileMenuItem.MenuItems.Add(ExitMenuItem);
            FileMenuItem.Name = "File";
            FileMenuItem.Text = "Файл";

            // Инициализация пункта меню "Выход"
            ExitMenuItem.Name = "Exit";
            ExitMenuItem.Text = "Выход";
            ExitMenuItem.Click += new EventHandler(Exit_Click);
        }

        /// <summary> Создание списка </summary>
        private ListView CreateListView(string[] Captions, int[] Proportions)
        {
            ListView lv = new ListView();
            lv.FullRowSelect = true;
            lv.Height = ClientSize.Height - 50;
            lv.Location = new Point(3, 0);
            lv.MultiSelect = false;
            lv.Parent = this;
            lv.View = View.Details;
            lv.Width = ClientSize.Width - 6;
            int i, sum = 0;
            for(i = 0; i < Captions.Length; i++) sum += Proportions[i];
            for(i = 0; i < Captions.Length; i++)
                lv.Columns.Add(Captions[i], (int)Math.Round((float)Proportions[i] / sum * (lv.Width - 6)));
            lv.Hide();
            return lv;
        }

        /// <summary> Создание кнопки </summary>
        private Button CreateButton(string Name, ButtonAlignment ba, int TabIndex)
        {
            Point p = new Point(0, 0);
            if (ba == ButtonAlignment.Left)
                p = new Point(4, ClientSize.Height - 47);
            if (ba == ButtonAlignment.Center)
                p = new Point((int)(ClientSize.Width - 6) / 2, ClientSize.Height - 47);
            if (ba == ButtonAlignment.Right)
                p = new Point(ClientSize.Width - 4 - 70, ClientSize.Height - 47);
            Button b = new Button();
            b.Location = p;
            b.Name = Name;
            b.Parent = this;
            b.Size = new Size(70, 25); ;
            b.TabIndex = TabIndex;
            b.Text = Name;
            b.Click += new EventHandler(Button_Click);
            b.Hide();
            return b;
        }
        #endregion

        /// <summary> Реакция на нажатие любой из кнопок </summary>
        void Button_Click(object sender, EventArgs e)
        {
            UserAction ua = UserAction.Undefined;
            if(((Button)sender).Text == "Выбрать") ua = UserAction.Select;
            if (((Button)sender).Text == "Добавить") ua = UserAction.Add;
            if (((Button)sender).Text == "Назад") ua = UserAction.Back;
            Manager(ua);
        }

        /// <summary> Управляющая процедура </summary>
        private void Manager(UserAction ua)
        {
            if (State == ProgramState.InitialState)
            {
                ProductsList.Show();
                Select.Show();
                Cache.FillCatalogs();
                UpdateLists();
                State = ProgramState.ProductsList;
            }

            if (State == ProgramState.ProductsList && ua == UserAction.Select)
            {
                if (ProductsList.SelectedIndices.Count == 0) return;
                if (ProductsList.SelectedItems[0].SubItems[3].Text == "") return;
                int prodIndex = ProductsList.SelectedIndices[0] / 6;
                string ProductID = (Cache.Products[prodIndex] as Product).ID;
                ProductsList.Hide();
                Select.Hide();
                UpdateRegistrationsList(ProductID);
                RegistrationsList.Show();
                Back.Show();
            }
        }

        /// <summary> Обновление списков </summary>
        public void UpdateLists()
        {
            // Список изделий
            if (Cache.Products != null)
            {
                ProductsList.BeginUpdate();
                ProductsList.Items.Clear();
                foreach (Product p in Cache.Products)
                {
                    string[] sa;
                    p.Registrations.Sort();
                    if (p.Registrations == null || p.Registrations.Count == 0)
                        throw new Exception("Так не должно быть");
                    Registration r = p.Registrations[p.Registrations.Count - 1] as Registration;
                    int defIndex = Cache.Deffects.BinarySearch(new Deffect(r.DeffectID, ""));
                    int worIndex = Cache.Workers.BinarySearch(new Worker(r.WorkerID, ""));
                    string time = r.Time.Hour.ToString() + ":" + r.Time.Minute.ToString();
                    string date = r.Time.ToShortDateString();
                    string line = r.Line.ToString();
                    string model = p.ModelID.ToString() + " (" + p.Model + ")";
                    string deffect = r.DeffectID.ToString() + " (" +
                        (Cache.Deffects[defIndex] as Deffect).Name + ")";
                    string worker = r.WorkerID.ToString() + " (" +
                        (Cache.Workers[worIndex] as Worker).Name + ")";
                    sa = new string[] { time, line, "Атрибут", "Значение" };
                    ProductsList.Items.Add(new ListViewItem(sa));
                    sa = new string[] { date, "", "Модель", model };
                    ProductsList.Items.Add(new ListViewItem(sa));
                    sa = new string[] { "", "", "Дефект", deffect };
                    ProductsList.Items.Add(new ListViewItem(sa));
                    sa = new string[] { "", "", "Оператор", worker };
                    ProductsList.Items.Add(new ListViewItem(sa));
                    sa = new string[] { "", "", "", "" };
                    ProductsList.Items.Add(new ListViewItem(sa));
                }
                ProductsList.EndUpdate();
            }

            // Список дефектов
            if (Cache.Deffects != null)
            {
                DeffectsList.BeginUpdate();
                DeffectsList.Items.Clear();
                foreach (Deffect d in Cache.Deffects)
                {
                    string[] sa = new string[] { d.ID.ToString(), d.Name };
                    DeffectsList.Items.Add(new ListViewItem(sa));
                }
                DeffectsList.EndUpdate();
            }

            // Список работников
            if (Cache.Workers != null)
            {
                WorkersList.BeginUpdate();
                WorkersList.Items.Clear();
                foreach (Worker w in Cache.Workers)
                {
                    string[] sa = new string[] { w.ID.ToString(), w.Name };
                    WorkersList.Items.Add(new ListViewItem(sa));
                }
                WorkersList.EndUpdate();
            }
        }
        
        /// <summary> Обновление списка регистраций для изделия </summary>
        public void UpdateRegistrationsList(string ProductID)
        {
            int prodIndex = Cache.Products.BinarySearch(new Product(ProductID, 0, "", null));
            RegistrationsList.BeginUpdate();
            RegistrationsList.Items.Clear();
            (Cache.Products[prodIndex] as Product).Registrations.Sort();
            foreach (Registration r in (Cache.Products[prodIndex] as Product).Registrations)
            {
                int defIndex = Cache.Deffects.BinarySearch(new Deffect(r.DeffectID, ""));
                int worIndex = Cache.Workers.BinarySearch(new Worker(r.WorkerID, ""));
                string time = r.Time.ToShortTimeString();
                string deffect = (Cache.Deffects[defIndex] as Deffect).ID.ToString() + " (" +
                    (Cache.Deffects[defIndex] as Deffect).Name + ")";
                string worker = (Cache.Workers[worIndex] as Worker).ID.ToString() + " (" +
                    (Cache.Workers[worIndex] as Worker).Name + ")";
                RegistrationsList.Items.Add(new ListViewItem(new string[] { time, deffect, worker }));
            }
            RegistrationsList.EndUpdate();
        }
        
        /// <summary> Выход из программы  </summary>
        void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    /// <summary> Настройки программы </summary>
    public static class ProgramSettings
    {
        public static TimeSpan ReentrancePeriod = new TimeSpan(0, 15, 0);
    }

    /// <summary> Вид дефекта </summary>
    public class Deffect : IComparable
    {
        public int ID;
        public string Name;

        public Deffect(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int CompareTo(object d)
        {
            return ID.CompareTo((d as Deffect).ID);
        }
    }

    /// <summary> Работник </summary>
    public class Worker : IComparable
    {
        public int ID;
        public string Name;

        public Worker(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int CompareTo(object w)
        {
            return ID.CompareTo((w as Worker).ID);
        }
    }

    /// <summary> Запись о деффекте </summary>
    public class Registration : IComparable
    {
        public DateTime Time;
        public int Line;
        public int DeffectID;
        public int WorkerID;
        public bool IsChanged;

        public Registration(DateTime time, int line, int deffectID, int workerID, bool isChanged)
        {
            Time = time;
            Line = line;
            DeffectID = deffectID;
            WorkerID = workerID;
            IsChanged = isChanged;
        }
        
        public int CompareTo(object r)
        {
            return Time.CompareTo((r as Registration).Time);
        }
    }

    /// <summary> Изделие </summary>
    public class Product : IComparable
    {
        public string ID;
        public int ModelID;
        public string Model;
        public ArrayList Registrations;

        public Product(string id, int modelID, string model, ArrayList registrations)
        {
            ID = id;
            ModelID = modelID;
            Model = model;
            Registrations = registrations;
        }

        public int CompareTo(object p)
        {
            return ID.CompareTo((p as Product).ID);
        }
    }
    
    /// <summary> Кэш </summary>
    public class Cache
    {
        public ArrayList Deffects;
        public ArrayList Workers;
        public ArrayList Products;

        public Cache()
        {
            Prods.Add(new Product("1111111111", 1, "Indesit111", null));
            Prods.Add(new Product("2222222222", 2, "Indesit222", null));
            Prods.Add(new Product("3333333333", 3, "Indesit333", null));
            Prods.Add(new Product("4444444444", 4, "Indesit444", null));
            Prods.Add(new Product("5555555555", 5, "Indesit555", null));
            Prods.Add(new Product("6666666666", 6, "Indesit666", null));
            Prods.Add(new Product("7777777777", 7, "Indesit777", null));
        }

        /// Для имитации работы с ССД
        private ArrayList Prods = new ArrayList();
        
        /// <summary> Заполнение справочников </summary>
        public void FillCatalogs()
        {
            Products = new ArrayList();
            Deffects = new ArrayList();
            Workers = new ArrayList();

            Deffects.Add(new Deffect(1, "Нет двери"));
            Deffects.Add(new Deffect(2, "Нет ручки"));
            Deffects.Add(new Deffect(3, "Царапина"));
            Deffects.Add(new Deffect(4, "Вмятина"));
            Workers.Add(new Worker(1, "Иванов И.И."));
            Workers.Add(new Worker(2, "Петров П.П."));
            
            ArrayList regs = new ArrayList();
            regs.Add(new Registration(DateTime.Now, 1, 1, 1, false));
            regs.Add(new Registration(DateTime.Now, 1, 2, 1, false));
            regs.Add(new Registration(DateTime.Now, 1, 3, 1, false));
            Products.Add(new Product("1111111111", 1, "Indesit111", regs));
        }

        /// <summary> Получение информации об изделии из БД </summary>
        public Product GetProductInfo(string ProductID)
        {
            for (int i = 0; i < Prods.Count; i++)
            {
                Product p = (Product)Prods[i];
                if (p.ID == ProductID)
                {
                    if(p.Registrations == null)
                        return new Product(p.ID, p.ModelID, p.Model, null);
                    ArrayList Regs = null;
                    for(int j = 0; j < p.Registrations.Count; j++)
                    {
                        Registration r = (Registration)p.Registrations[j];
                        if (DateTime.Now < r.Time.Add(ProgramSettings.ReentrancePeriod))
                        {
                            r = new Registration(r.Time, r.Line, r.DeffectID, r.WorkerID, r.IsChanged);
                            if (Regs == null) Regs = new ArrayList();
                            Regs.Add(r);
                        }
                    }
                    return new Product(p.ID, p.ModelID, p.Model, Regs);
                }
            }
            return null;
        }

        /// <summary> Добавление записи о деффекте в БД </summary>
        public void AddRegistration(string ProductID, Registration r)
        {
            for (int i = 0; i < Prods.Count; i++)
            {
                Product p = (Product)Prods[i];
                if (p.ID == ProductID)
                {
                    r = new Registration(r.Time, r.Line, r.DeffectID, r.WorkerID, r.IsChanged);
                    p.Registrations.Add(r);
                }
            }
        }
    }
}