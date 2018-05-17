using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Terminal.API;

namespace Terminal
{
    /// <summary>
    /// Главная форма для режима работы "вход-выход"
    /// </summary>
    public partial class MainFormDeffectsOnly : Form
    {
        #region Элементы управления
        ListView ProdsList;
        ListView RegsList;
        ListView DefsList;
        ListView WorksList;
        Button LeftButton;
        Button CenterButton;
        Button RightButton;
        StatusBar StatusBar;
        MainMenu MainMenu;
        MenuItem FileMenuItem;
        MenuItem ExitMenuItem;
        MenuItem EditMenuItem;
        MenuItem EnterIDMenuItem;
        #endregion

        ImageList ImageList;
        Cache Cache;
        ProgramState State;
        InputForm InputForm;
        AcceptionForm AcceptionForm;
        SelectActionForm SelectActionForm;
        AuthentificationForm AuthentificationForm;

        string ScannedID;
        delegate void IntScIDDelegate(string scanned);
        IntScIDDelegate Delegate;

        DcdHandle DcdHandle;
        private ImageList imageList1;
        DcdEvent DcdEvent;
        DcdScanned DcdScannedDelegate;
        

        /// <summary>
        /// Действие пользователя
        /// </summary>
        enum UserAction
        { Add, Select, Back, Change, Cancel, Finish, Undefined, Scan, Next, EnterCode }

        /// <summary>
        /// Положение кнопки
        /// </summary>
        enum ButtonPosition
        { Left, Right, Center }

        /// <summary>
        /// Состояние работы программы
        /// </summary>
        enum ProgramState
        { InitialState, ProdsList, RegsList, DefsList, DefsListEditing, WorksListEditing, WorksList }

        #region Инициализация
        /// <summary> Инициализация компонентов </summary>
        public MainFormDeffectsOnly()
        {
            SuspendLayout();
            
            MainMenu = new MainMenu();
            FileMenuItem = new MenuItem();
            ExitMenuItem = new MenuItem();
            EditMenuItem = new MenuItem();
            EnterIDMenuItem = new MenuItem();
            StatusBar = new StatusBar();
            
            // Инициализация формы
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Menu = MainMenu;
            Name = "MainForm";
            Text = "ICS Reworks: сбор данных";
            WindowState = FormWindowState.Maximized;
            Load += new EventHandler(MainForm_Load);
            Closing += new CancelEventHandler(MainFormDeffectsOnly_Closing);

            // Инициализация меню
            MainMenu.MenuItems.Add(FileMenuItem);
            MainMenu.MenuItems.Add(EditMenuItem);
            FileMenuItem.MenuItems.Add(ExitMenuItem);
            EditMenuItem.MenuItems.Add(EnterIDMenuItem);
            FileMenuItem.Text = "Файл";
            ExitMenuItem.Text = "Выход";
            EditMenuItem.Text = "Правка";
            EnterIDMenuItem.Text = "Ввести штрих-код...";
            ExitMenuItem.Click += new EventHandler(ExitClick);
            EnterIDMenuItem.Click += new EventHandler(EnterID);

            // Инициализация строки состояния
            StatusBar.Location = new Point(0, ClientSize.Height - StatusBar.Height);
            StatusBar.Name = "StatusBar";
            StatusBar.Parent = this;
            StatusBar.Text = "Строка состояния";

            ResumeLayout();
        }

        /// <summary> Дополнительная инициализация компонентов </summary>
        void MainForm_Load(object sender, EventArgs e)
        {
            ImageList = new ImageList();
            Cache = new Cache("gg");
            Cache.Line = ProgramSettings.Line;
            InputForm = new InputForm();
            AcceptionForm = new AcceptionForm();
            SelectActionForm = new SelectActionForm();
            AuthentificationForm = new AuthentificationForm();
            Delegate = new IntScIDDelegate(InterpretScannedID);
            State = ProgramState.InitialState;

            SuspendLayout();

            // Инициализация списков
            string[] Captions = new string[] { "Дата", "Линия", "Атрибут", "Значение" };
            int[] Proportions = new int[] { 2, 2, 3, 4 };
            ProdsList = CreateListView(Captions, Proportions);
            
            //ProdsList.SmallImageList = AuthentificationForm.imageList;

            Captions = new string[] { "Время", "Дефект", "Оператор" };
            Proportions = new int[] { 3, 5, 5 };
            RegsList = CreateListView(Captions, Proportions);
            Captions = new string[] { "Код", "Дефект" };
            Proportions = new int[] { 2, 5 };
            DefsList = CreateListView(Captions, Proportions);
            Captions = new string[] { "Код", "Оператор" };
            Proportions = new int[] { 2, 5 };
            WorksList = CreateListView(Captions, Proportions);

            // Инициализация кнопок
            LeftButton = CreateButton(ButtonPosition.Left, 11);
            CenterButton = CreateButton(ButtonPosition.Center, 21);
            RightButton = CreateButton(ButtonPosition.Right, 31);

            ResumeLayout();
            Manager(UserAction.Undefined);
        }

         /// <summary> Создание списка </summary>
        private ListView CreateListView(string[] Captions, int[] Proportions)
        {
            ListView lv = new ListView();
            int i, w, sum = 0;
            lv.FullRowSelect = true;
            lv.Height = ClientSize.Height - 80;
            lv.Location = new Point(3, 28);
            lv.Parent = this;
            lv.View = View.Details;
            lv.Width = ClientSize.Width - 6;
            for (i = 0; i < Proportions.Length; i++) sum += Proportions[i];
            for (i = 0; i < Captions.Length; i++)
            {
                w = (int)Math.Round((float)Proportions[i] / sum * (lv.Width - 6));
                lv.Columns.Add(Captions[i], w, HorizontalAlignment.Left);
            }
            lv.Hide();
            return lv;
        }

        /// <summary> Создание кнопки </summary>
        private Button CreateButton(ButtonPosition ba, int TabIndex)
        {
            Point p = new Point(0, 0);
            if (ba == ButtonPosition.Left)
                p = new Point(4, ClientSize.Height - 49);
            if (ba == ButtonPosition.Center)
                p = new Point((int)ClientSize.Width / 2 - 37, ClientSize.Height - 49);
            if (ba == ButtonPosition.Right)
                p = new Point(ClientSize.Width - 4 - 74, ClientSize.Height - 49);
            Button b = new Button();
            b.Location = p;
            b.Parent = this;
            b.Size = new Size(74, 25);
            b.TabIndex = TabIndex;
            b.Text = "";
            b.Click += new EventHandler(ButtonClick);
            b.Hide();
            return b;
        }
        #endregion

        /// <summary>
        /// Реакция на нажатие любой из кнопок
        /// </summary>
        void ButtonClick(object sender, EventArgs e)
        {
            UserAction ua = UserAction.Undefined;
            if (((Button)sender).Text == "Выбрать") ua = UserAction.Select;
            if (((Button)sender).Text == "Добавить") ua = UserAction.Add;
            if (((Button)sender).Text == "Отмена") ua = UserAction.Cancel;
            if (((Button)sender).Text == "Править") ua = UserAction.Change;
            if (((Button)sender).Text == "Завершить") ua = UserAction.Finish;
            if (((Button)sender).Text == "Далее") ua = UserAction.Next;
            if (((Button)sender).Text == "Назад") ua = UserAction.Back;
            Manager(ua);
        }

        /// <summary>
        /// Ручной ввод штрих-кода (в любом состоянии)
        /// </summary>
        void EnterID(object sender, EventArgs e)
        {
            InputForm.textBox1.Text = "";
            if (State == ProgramState.ProdsList)
            {
                InputForm.Text = "Список изделий";
                InputForm.label1.Text = "Введите штрих-код изделия:";
            }
            if (State == ProgramState.RegsList)
            {
                InputForm.Text = "Список регистраций";
                InputForm.label1.Text = "Введите штрих-код дефекта:";
            }
            if (State == ProgramState.DefsList || State == ProgramState.DefsListEditing)
            {
                InputForm.Text = "Список дефектов";
                InputForm.label1.Text = "Введите штрих-код дефекта:";
            }
            if (State == ProgramState.WorksList || State == ProgramState.WorksListEditing)
            {
                InputForm.Text = "Список операторов";
                InputForm.label1.Text = "Введите штрих-код оператора:";
            }
            if (InputForm.ShowDialog() == DialogResult.OK)
            {
                ScannedID = InputForm.textBox1.Text;
                Manager(UserAction.EnterCode);
            }
        }

        /// <summary>
        /// Другой поток: обработка события "Код сканирован" и переход к потоку данного окна
        /// </summary>
        void DcdEvent_Scanned(object sender, DcdEventArgs e)
        {
            try
            {
                //DcdEvent.Scanned -= DcdScannedDelegate;
                if (DcdHandle.GetCodeId(e.RequestID) == CodeId.NoData)
                    throw new Exception("Данные не считаны");
                string scanned = DcdHandle.ReadString(e.RequestID);
                scanned = scanned.Substring(0, scanned.Length - 1);
                
                //this.Invoke(Delegate, new object[] { scanned });
                InterpretScannedID(scanned);
                //DcdEvent.Scanned += DcdScannedDelegate;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Сканирование штрих-кода",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                return;
            }
        }

        /// <summary>
        /// Обработка события "Код сканирован"
        /// </summary>
        void InterpretScannedID(string scanned)
        {
            ScannedID = scanned;
            Manager(UserAction.Scan);
        }

        /// <summary>
        /// Показ нужных элементов управления
        /// </summary>
        void ModifyLayout(ListView Next, ListView Prev, string l, string c, string r)
        {
            SuspendLayout();
            if (Prev != null) Prev.Hide();
            if (Next != null)
            {
                Next.Show();
                Next.Focus();
            }
            if (l != "")
            {
                LeftButton.Text = l;
                LeftButton.Show();
            }
            else if (LeftButton.Text != "") LeftButton.Hide();
            if (c != "")
            {
                CenterButton.Text = c;
                CenterButton.Show();
            }
            else if (CenterButton.Text != "") CenterButton.Hide();
            if (r != "")
            {
                RightButton.Text = r;
                RightButton.Show();
            }
            else if (RightButton.Text != "") RightButton.Hide();
            ResumeLayout();
        }

        /// <summary>
        /// Управляющая процедура
        /// </summary>
        void Manager(UserAction ua)
        {
            try
            {
                //if (DcdEvent != null && State != ProgramState.InitialState)
                //    DcdEvent.Scanned -= DcdScannedDelegate;

                #region Начальное состояние
                // Начальное состояние
                if (State == ProgramState.InitialState)
                {
                    Cache.UpdateCatalogs();
                    FillProdsList();
                    FillDefsList();
                    FillWorksList();
                    string wID, passw;
                    while (true)
                    {
                        AuthentificationForm.ShowDialog();
                        if (AuthentificationForm.DialogResult != DialogResult.OK)
                            Close();
                        wID = AuthentificationForm.textBox1.Text;
                        passw = AuthentificationForm.textBox2.Text;
                        if (Cache.VerifyPassword(wID, passw) == true) break;
                        else MessageBox.Show("Неверный код пользователя или пароль", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button1);
                    }

                    if (MessageBox.Show("Сканер работает?", "Отладка",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {

                        DcdHandle = new DcdHandle(DcdDeviceCap.Barcode);
                        DcdEvent = new DcdEvent(DcdHandle, DcdRequestType.PostRecurring, this);
                        DcdHandle.SetReadAhead(false);
                        DcdScannedDelegate = new DcdScanned(DcdEvent_Scanned);
                        DcdEvent.Scanned += DcdScannedDelegate;
                    }

                    ModifyLayout(ProdsList, null, "Добавить", "Выбрать", "Обновить");
                    State = ProgramState.ProdsList;
                    return;
                }
                #endregion

                #region Список изделий
                // Сканирован код изделия (или введен вручную)
                if (State == ProgramState.ProdsList &&
                    (ua == UserAction.Scan || ua == UserAction.EnterCode))
                {
                    Cache.SelectedProdID = ScannedID;
                    if (Cache.VerifyProdID() == false)
                    {
                        Cache.UpdateProdInfo();
                        if (Cache.VerifyProdID() == false)
                            throw new Exception("Неверный код изделия");
                    }
                    Cache.FillTempRegs();
                    FillRegsList();
                    if (RegsList.Items.Count != 0)
                    {
                        ModifyLayout(RegsList, ProdsList, "Добавить", "Править", "Завершить");
                        State = ProgramState.RegsList;
                    }
                    else
                    {
                        ModifyLayout(DefsList, ProdsList, "Выбрать", "Отмена", "");
                        State = ProgramState.DefsList;
                    }
                    return;
                }

                // Ручной ввод кода изделия (по кнопке "Добавить")
                if (State == ProgramState.ProdsList && ua == UserAction.Add)
                {
                    InputForm.textBox1.Text = "";
                    InputForm.Text = "Список изделий";
                    InputForm.label1.Text = "Введите штрих-код изделия:";
                    InputForm.ShowDialog();
                    if (InputForm.DialogResult == DialogResult.OK)
                    {
                        Cache.SelectedProdID = InputForm.textBox1.Text;
                        if (Cache.VerifyProdID() == false)
                        {
                            Cache.UpdateProdInfo();
                            if (Cache.VerifyProdID() == false)
                                throw new Exception("Неверный код изделия");
                        }
                        Cache.FillTempRegs();
                        FillRegsList();
                        if (RegsList.Items.Count != 0)
                        {
                            ModifyLayout(RegsList, ProdsList, "Добавить", "Править", "Завершить");
                            State = ProgramState.RegsList;
                        }
                        else
                        {
                            ModifyLayout(DefsList, ProdsList, "Выбрать", "Отмена", "");
                            State = ProgramState.DefsList;
                        }
                    }
                    return;
                }

                // Выбрано изделие из списка
                if (State == ProgramState.ProdsList && ua == UserAction.Select)
                {
                    SetSelectedProdID();
                    if (Cache.VerifyProdID() == false)
                        throw new Exception("Неверный код изделия");
                    Cache.FillTempRegs();
                    FillRegsList();
                    if (RegsList.Items.Count != 0)
                    {
                        ModifyLayout(RegsList, ProdsList, "Добавить", "Править", "Завершить");
                        State = ProgramState.RegsList;
                    }
                    else
                    {
                        ModifyLayout(DefsList, ProdsList, "Выбрать", "Отмена", "");
                        State = ProgramState.DefsList;
                    }
                    return;
                }
                #endregion

                #region Список регистраций
                // Код сканирован или введен вручную
                if (State == ProgramState.RegsList &&
                    (ua == UserAction.Scan || ua == UserAction.EnterCode))
                {
                    Cache.SelectedDefID = ScannedID;
                    if (Cache.VerifyDefID() == false)
                        throw new Exception("Неверный код дефекта");
                    if (Cache.DefAlreadyRegistered() == true)
                    {
                        if (Cache.GetTempRegState() != "NewTemp")
                            throw new Exception("Невозможно править сохраненную запись");
                        SelectActionForm.Text = "Правка записи";
                        SelectActionForm.label1.Text = "Выберете действие:";
                        SelectActionForm.button1.Text = "Изменить";
                        SelectActionForm.button2.Text = "Удалить";
                        SelectActionForm.button3.Text = "Отмена";
                        SelectActionForm.ShowDialog();
                        if (SelectActionForm.DialogResult == DialogResult.Cancel)
                            return;

                        // Удалить запись
                        if (SelectActionForm.DialogResult == DialogResult.No)
                        {
                            AcceptionForm.Text = "Удаление записи";
                            AcceptionForm.label1.Text = "Удалить запись?";
                            AcceptionForm.ShowDialog();
                            if (AcceptionForm.DialogResult == DialogResult.Yes)
                            {
                                Cache.DelTempReg();
                                RemoveRegsListRecord();
                            }
                        }
                        // Изменить запись
                        if (SelectActionForm.DialogResult == DialogResult.Yes)
                        {
                            ModifyLayout(DefsList, RegsList, "Выбрать", "Далее", "Отмена");
                            State = ProgramState.DefsListEditing;
                        }
                    }
                    else
                    {
                        SelectDefsListRecord();
                        ModifyLayout(WorksList, RegsList, "Выбрать", "Назад", "Отмена");
                        State = ProgramState.WorksList;
                    }
                    return;
                }

                // Выход из списка регистраций
                if (State == ProgramState.RegsList && ua == UserAction.Finish)
                {
                    if (Cache.TempRegsAdded() == true)
                    {
                        SelectActionForm.Text = "Сохранение изменений";
                        SelectActionForm.label1.Text = "Сохранить изменения?";
                        SelectActionForm.button1.Text = "Да";
                        SelectActionForm.button2.Text = "Нет";
                        SelectActionForm.button3.Text = "Отмена";
                        SelectActionForm.ShowDialog();
                        if (SelectActionForm.DialogResult == DialogResult.Cancel)
                            return;
                        if (SelectActionForm.DialogResult == DialogResult.Yes)
                        {
                            Cache.AcceptTempRegs();
                            TryToRemoveProdsListRecord();
                            if ((ProdsList.Items.Count + 1) / 5 > ProgramSettings.MaxProdsListRecords)
                                RemoveFirstProdsListRecord();
                            AddProdsListRecord();
                        }
                    }
                    ModifyLayout(ProdsList, RegsList, "Добавить", "Выбрать", "Обновить");
                    State = ProgramState.ProdsList;
                    return;
                }

                // Добавление записи о дефекте
                if (State == ProgramState.RegsList && ua == UserAction.Add)
                {
                    ModifyLayout(DefsList, RegsList, "Выбрать", "Отмена", "");
                    State = ProgramState.DefsList;
                    return;
                }

                // Изменение записи о дефекте
                if (State == ProgramState.RegsList && ua == UserAction.Change)
                {
                    SetSelectedReg();
                    if (Cache.GetTempRegState() != "NewTemp")
                        throw new Exception("Невозможно править сохраненную запись");
                    SelectActionForm.Text = "Правка записи";
                    SelectActionForm.label1.Text = "Выберете действие:";
                    SelectActionForm.button1.Text = "Изменить";
                    SelectActionForm.button2.Text = "Удалить";
                    SelectActionForm.button3.Text = "Отмена";
                    SelectActionForm.ShowDialog();
                    if (SelectActionForm.DialogResult == DialogResult.Cancel)
                        return;

                    // Удалить запись
                    if (SelectActionForm.DialogResult == DialogResult.No)
                    {
                        AcceptionForm.Text = "Удаление записи";
                        AcceptionForm.label1.Text = "Удалить запись?";
                        AcceptionForm.ShowDialog();
                        if (AcceptionForm.DialogResult == DialogResult.Yes)
                        {
                            Cache.DelTempReg();
                            RemoveRegsListRecord();
                        }
                    }
                    // Изменить запись
                    if (SelectActionForm.DialogResult == DialogResult.Yes)
                    {
                        ModifyLayout(DefsList, RegsList, "Выбрать", "Далее", "Отмена");
                        State = ProgramState.DefsListEditing;
                    }
                    return;
                }
                #endregion

                #region Список дефектов
                // Код дефекта введен или сканирован
                if (State == ProgramState.DefsList &&
                    (ua == UserAction.Scan || ua == UserAction.EnterCode))
                {
                    Cache.SelectedDefID = ScannedID;
                    if (Cache.VerifyDefID() == false)
                        throw new Exception("Неверный код дефекта");
                    if (Cache.DefAlreadyRegistered() == true)
                        throw new Exception("Дефект уже зарегистрирован");
                    SelectDefsListRecord();
                    ModifyLayout(WorksList, DefsList, "Выбрать", "Назад", "Отмена");
                    State = ProgramState.WorksList;
                    return;
                }

                // Код дефекта введен или сканирован (в режиме редактирования записи)
                if (State == ProgramState.DefsListEditing &&
                    (ua == UserAction.Scan || ua == UserAction.EnterCode))
                {
                    Cache.SelectedDefID = ScannedID;
                    if (Cache.VerifyDefID() == false)
                        throw new Exception("Неверный код дефекта");
                    if (Cache.DefAlreadyRegistered() == true)
                        throw new Exception("Дефект уже зарегистрирован");
                    SelectDefsListRecord();
                    ModifyLayout(WorksList, DefsList, "Выбрать", "Назад", "Отмена");
                    State = ProgramState.WorksListEditing;
                    return;
                }

                // Выход из режима добавления дефекта
                if ((State == ProgramState.DefsList || State == ProgramState.DefsListEditing)
                    && ua == UserAction.Cancel)
                {
                    ModifyLayout(RegsList, DefsList, "Добавить", "Править", "Завершить");
                    State = ProgramState.RegsList;
                    return;
                }

                // Дефект выбран
                if (State == ProgramState.DefsList && ua == UserAction.Select)
                {
                    SetSelectedDefID();
                    if (Cache.DefAlreadyRegistered() == true)
                        throw new Exception("Дефект уже зарегистрирован");
                    ModifyLayout(WorksList, DefsList, "Выбрать", "Назад", "Отмена");
                    State = ProgramState.WorksList;
                    return;
                }

                // Деффект выбран (в режиме редактирования записи)
                if (State == ProgramState.DefsListEditing && ua == UserAction.Select)
                {
                    SetSelectedDefID();
                    if (Cache.DefAlreadyRegistered() == true)
                        throw new Exception("Дефект уже зарегистрирован");
                    ModifyLayout(WorksList, DefsList, "Выбрать", "Далее", "Отмена");
                    State = ProgramState.WorksListEditing;
                    return;
                }

                // Деффект пропущен (в режиме редактирования записи)
                if (State == ProgramState.DefsListEditing && ua == UserAction.Next)
                {
                    SelectDefsListRecord();
                    ModifyLayout(WorksList, DefsList, "Выбрать", "Далее", "Отмена");
                    State = ProgramState.WorksListEditing;
                    return;
                }
                #endregion

                #region Список операторов
                // Код оператора введен или сканирован
                if (State == ProgramState.WorksList &&
                    (ua == UserAction.Scan || ua == UserAction.EnterCode))
                {
                    Cache.SelectedWorkID = ScannedID;
                    if (Cache.VerifyWorkID() == false)
                        throw new Exception("Неверный код оператора");
                    SelectWorksListRecord();
                    SetSelectedDefID();

                    Cache.AddTempReg();
                    AddRegsListRecord();
                    ModifyLayout(RegsList, WorksList, "Добавить", "Править", "Завершить");
                    State = ProgramState.RegsList;
                    return;
                }

                // Код оператора введен или сканирован (в режиме редактирования записи)
                if (State == ProgramState.WorksListEditing &&
                    (ua == UserAction.Scan || ua == UserAction.EnterCode))
                {
                    Cache.SelectedWorkID = ScannedID;
                    if (Cache.VerifyWorkID() == false)
                        throw new Exception("Неверный код оператора");
                    SelectWorksListRecord();
                    SetSelectedReg();
                    RemoveRegsListRecord();
                    Cache.DelTempReg();

                    SetSelectedDefID();
                    SetSelectedWorkID();
                    Cache.AddTempReg();
                    AddRegsListRecord();
                    ModifyLayout(RegsList, WorksList, "Добавить", "Править", "Завершить");
                    State = ProgramState.RegsList;
                    return;
                }

                // Назад к выбору дефекта
                if (State == ProgramState.WorksList && ua == UserAction.Back)
                {
                    ModifyLayout(DefsList, WorksList, "Выбрать", "Отмена", "");
                    State = ProgramState.DefsList;
                    return;
                }

                // Отмена добавления записи о дефекте
                if ((State == ProgramState.WorksList || State == ProgramState.WorksListEditing)
                    && ua == UserAction.Cancel)
                {
                    ModifyLayout(RegsList, WorksList, "Добавить", "Править", "Завершить");
                    State = ProgramState.RegsList;
                    return;
                }

                // Оператор выбран
                if (State == ProgramState.WorksList && ua == UserAction.Select)
                {
                    SetSelectedDefID();
                    SetSelectedWorkID();

                    Cache.AddTempReg();
                    AddRegsListRecord();
                    ModifyLayout(RegsList, WorksList, "Добавить", "Править", "Завершить");
                    State = ProgramState.RegsList;
                    return;
                }

                // Оператор выбран (в режиме редактирования записи)
                if (State == ProgramState.WorksListEditing && ua == UserAction.Select)
                {
                    SetSelectedReg();
                    RemoveRegsListRecord();
                    Cache.DelTempReg();

                    SetSelectedDefID();
                    SetSelectedWorkID();
                    Cache.AddTempReg();
                    AddRegsListRecord();
                    ModifyLayout(RegsList, WorksList, "Добавить", "Править", "Завершить");
                    State = ProgramState.RegsList;
                    return;
                }

                // Оператор пропущен (в режиме редактирования записи)
                if (State == ProgramState.WorksListEditing && ua == UserAction.Next)
                {
                    SelectWorksListRecord();
                    RemoveRegsListRecord();
                    Cache.DelTempReg();

                    SetSelectedDefID();
                    Cache.AddTempReg();
                    AddRegsListRecord();
                    ModifyLayout(RegsList, WorksList, "Добавить", "Править", "Завершить");
                    State = ProgramState.RegsList;
                    return;
                }
                #endregion

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
            }
            finally
            {
                //if (DcdEvent != null)
                //{
                //    DcdEvent.Scanned += DcdScannedDelegate;
                //}
            }
        }

        #region Вспомогательные функции для работы со списками
        /// <summary>
        /// Заполнение списка изделий
        /// </summary>
        public void FillProdsList()
        {
            ProdsList.BeginUpdate();
            ProdsList.Items.Clear();
            ListViewItem[] arr = Cache.GetProdsListContents();
            foreach (ListViewItem lvi in arr)
                ProdsList.Items.Add(lvi);
            ProdsList.EndUpdate();
        }

        /// <summary>
        /// Заполнение списка регистраций
        /// </summary>
        public void FillRegsList()
        {
            RegsList.BeginUpdate();
            RegsList.Items.Clear();
            ListViewItem[] arr = Cache.GetRegsListContents();
            foreach (ListViewItem lvi in arr)
                RegsList.Items.Add(lvi);
            RegsList.EndUpdate();
        }

        /// <summary>
        /// Заполнение списка дефектов
        /// </summary>
        public void FillDefsList()
        {
            DefsList.BeginUpdate();
            DefsList.Items.Clear();
            ListViewItem[] arr = Cache.GetDefsListContents();
            foreach (ListViewItem lvi in arr)
                DefsList.Items.Add(lvi);
            DefsList.EndUpdate();
        }

        /// <summary>
        /// Заполнение списка операторов
        /// </summary>
        public void FillWorksList()
        {
            WorksList.BeginUpdate();
            WorksList.Items.Clear();
            ListViewItem[] arr = Cache.GetWorksListContents();
            foreach (ListViewItem lvi in arr)
                WorksList.Items.Add(lvi);
            WorksList.EndUpdate();
        }

        /// <summary>
        /// Добавление записи в список изделий
        /// </summary>
        public void AddProdsListRecord()
        {
            ProdsList.BeginUpdate();
            ListViewItem[] arr = Cache.GetProdsListRecord();
            ProdsList.Items.Add(new ListViewItem(new string[] { "", "", "", "" }));
            foreach (ListViewItem lvi in arr)
                ProdsList.Items.Add(lvi);
            ProdsList.EndUpdate();
        }

        /// <summary>
        /// Удаление первой записи из списка изделий
        /// </summary>
        public void RemoveFirstProdsListRecord()
        {
            ProdsList.BeginUpdate();
            for (int i = 0; i < 5; i++)
                ProdsList.Items.RemoveAt(0);
            ProdsList.EndUpdate();
        }

        /// <summary>
        /// Попытка удаления выбранной записи из списка изделий
        /// </summary>
        public void TryToRemoveProdsListRecord()
        {
            ProdsList.BeginUpdate();
            for (int i = 0; i < ProdsList.Items.Count; i += 5)
                if (ProdsList.Items[i].SubItems[3].Text == Cache.SelectedProdID)
                {
                    if (i == ProdsList.Items.Count - 4) i--;
                    for (int j = 0; j < 5; j++)
                        ProdsList.Items.RemoveAt(i);
                    break;
                }
            ProdsList.EndUpdate();
        }

        /// <summary>
        /// Добавление записи в список регистраций
        /// </summary>
        public void AddRegsListRecord()
        {
            RegsList.BeginUpdate();
            RegsList.Items.Add(Cache.GetRegsListRecord());
            RegsList.EndUpdate();
        }

        /// <summary>
        /// Удалить запись из списка регистраций
        /// </summary>
        public void RemoveRegsListRecord()
        {
            string dID = Cache.SelectedDefID;
            string wID = Cache.SelectedWorkID;
            foreach (ListViewItem lvi in RegsList.Items)
            {
                string def = lvi.SubItems[1].Text, work = lvi.SubItems[2].Text;
                if (def.Length < dID.Length || work.Length < wID.Length) continue;
                if (def.Substring(0, dID.Length) == dID &&
                    work.Substring(0, wID.Length) == wID)
                {
                    RegsList.Items.Remove(lvi);
                    break;
                }
            }
        }

        /// <summary>
        /// Установить код изделия, которое было выбрано из списка
        /// </summary>
        public void SetSelectedProdID()
        {
            int index = ProdsList.SelectedIndices[0];
            index -= index % 5;
            Cache.SelectedProdID = ProdsList.Items[index].SubItems[3].Text;
        }

        /// <summary>
        /// Установить код дефекта, который был выбран из списка
        /// </summary>
        public void SetSelectedDefID()
        {
            if (DefsList.SelectedIndices.Count != 1)
                throw new Exception("Не выбран дефект");
            int index = DefsList.SelectedIndices[0];
            Cache.SelectedDefID = DefsList.Items[index].SubItems[0].Text;
        }

        /// <summary>
        /// Установить код оператора, который был выбран из списка
        /// </summary>
        public void SetSelectedWorkID()
        {
            if (WorksList.SelectedIndices.Count != 1)
                throw new Exception("Не выбран оператор");
            int index = WorksList.SelectedIndices[0];
            Cache.SelectedWorkID = WorksList.Items[index].SubItems[0].Text;
        }

        /// <summary>
        /// Установить код дефекта и код оператора регистрации, выбранной из списка
        /// </summary>
        public void SetSelectedReg()
        {
            if (RegsList.SelectedIndices.Count != 1)
                throw new Exception("Не выбрана запись");
            int index = RegsList.SelectedIndices[0];
            string def, work;
            string[] tmp;
            def = RegsList.Items[index].SubItems[1].Text;
            tmp = def.Split(new char[] {' '});
            Cache.SelectedDefID = tmp[0];
            work = RegsList.Items[index].SubItems[2].Text;
            tmp = work.Split(new char[] { ' ' });
            Cache.SelectedWorkID = tmp[0];
        }

        /// <summary>
        /// Сделать дефект выбранным в списке дефектов
        /// </summary>
        public void SelectDefsListRecord()
        {
            foreach (ListViewItem lvi in DefsList.Items)
                if (lvi.SubItems[0].Text == Cache.SelectedDefID)
                    lvi.Selected = true;
                else lvi.Selected = false;
        }

        /// <summary>
        /// Сделать оператора выбранным в списке операторов
        /// </summary>
        public void SelectWorksListRecord()
        {
            foreach (ListViewItem lvi in WorksList.Items)
                if (lvi.SubItems[0].Text == Cache.SelectedWorkID)
                    lvi.Selected = true;
                else lvi.Selected = false;
        }
        #endregion

        /// <summary>
        /// Выключение сканера
        /// </summary>
        void MainFormDeffectsOnly_Closing(object sender, CancelEventArgs e)
        {
            if (DcdEvent == null) return;
            if (DcdEvent.IsListening) DcdEvent.StopScanListener();
        }
        
        /// <summary>
        /// Выход из программы
        /// </summary>
        void ExitClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Код, сгенерированный дизайнером
        /// </summary>
        void InitializeComponent()
        {
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            // 
            // MainFormDeffectsOnly
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            //this.ClientSize = new System.Drawing.Size(638, 455);
            this.Name = "MainFormDeffectsOnly";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }
    }

    /// <summary>
    /// Настройки программы
    /// </summary>
    public static class ProgramSettings
    {
        public static TimeSpan ReentrancePeriod = new TimeSpan(10, 15, 0);
        public static int Line = 1;
        public static int MaxProdsListRecords = 10;
        public static bool PermWorker = false;
        public static string PermWorkerID = "2";
    }
}