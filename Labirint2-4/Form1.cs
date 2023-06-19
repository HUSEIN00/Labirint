namespace Labirint2
{
    enum TypeFigure
    {
        Empty,
        Wall,
        Man,
        Target,
        Bomb
    }
    
    public partial class Form1 : Form
    {
        int Hour = 0;
        int Min = 0;
        int Sec = 0;
        int ColumnCount = 0;
        int RowCount = 0;
        // Координаты человека
        int numCurRowMan = 0;
        int numCurColMan = 0;
        string pathImageWall = "C:\\Game\\Wall.jpg";
        string pathImageMan = "C:\\Game\\Man.png";
        string pathImagebomb = "C:\\Game\\Man2.png";
        string pathImageTarget = "C:\\Game\\Цель2.jpg";
        Image imageWall = null;
        Image imageTarget = null;
        Image imageMan = null;
        Image imagebomb = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void fill_row(int NumRow, Image img, TypeFigure figure)
        {
            // Заполнить строку с индексом NumRow картинкой img
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Rows[NumRow].Cells[i].Value = img;
                dataGridView1.Rows[NumRow].Cells[i].Tag = figure;                 
            }
        }

        private void fill_column(int NumColumn, Image img, TypeFigure figure)
        {
            // Заполнить столбец с индексом NumColumn картинкой img
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[NumColumn].Value = img;
                dataGridView1.Rows[i].Cells[NumColumn].Tag = figure;

            }
        }

        private void fill_row_random(int NumRow, Random rnd)
        {
            int maxCountWall = (int)numericUpDown1.Value;
            int maxCountEmpty = (int)numericUpDown2.Value;
            int countWall = 0;
            int countEmpty = 0;

            int x = rnd.Next(0, 2);
            if (x == 0)
            {
                countEmpty = rnd.Next(1, maxCountEmpty + 1);
            }
            else
            {
                countWall = rnd.Next(1, maxCountWall + 1);
            }
            
            for (int i = 1; i < dataGridView1.ColumnCount - 1; i++)
            {
                if (countWall > 0)
                {
                    dataGridView1.Rows[NumRow].Cells[i].Value = imageWall;
                    dataGridView1.Rows[NumRow].Cells[i].Tag = TypeFigure.Wall;
                    countWall--;
                    if (countWall == 0)
                    {
                        countEmpty = rnd.Next(1, maxCountEmpty + 1);
                        continue;
                    }
                }

                if (countEmpty > 0)
                {
                    dataGridView1.Rows[NumRow].Cells[i].Value = null;
                    dataGridView1.Rows[NumRow].Cells[i].Tag = TypeFigure.Empty;
                    countEmpty--;
                    if (countEmpty == 0)
                    {
                        countWall = rnd.Next(1, maxCountWall + 1);

                    }
                }
            }
        }
        
        private void fill_Bomb(int NumRow, Random rnd, int countBomb) 
        {


            int x = rnd.Next(1, dataGridView1.ColumnCount - 1);
            for (int i = 1; i < dataGridView1.ColumnCount - 1; i++)
            {
                
                if ((TypeFigure)dataGridView1.Rows[NumRow].Cells[x].Tag == TypeFigure.Empty || (TypeFigure)dataGridView1.Rows[NumRow].Cells[x].Tag == TypeFigure.Wall)
                {
                    dataGridView1.Rows[NumRow].Cells[x].Value = imagebomb;
                    dataGridView1.Rows[NumRow].Cells[x].Tag = TypeFigure.Bomb;
                    countBomb--;
                    break;
                }
                if (countBomb == 0)
                {
                    break;
                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Кол-во строк или столбцов пусто!", "Ошибка");
                return;
            }
            
            // Кол-во строк
            RowCount = int.Parse(textBox1.Text.Trim());
            // Кол-во столбцов
            ColumnCount = int.Parse(textBox2.Text.Trim());
                       
            // Добавление колонок с возможностью вставки картинок
            for (int i = 0; i < ColumnCount; i++)
            {
                DataGridViewImageColumn dataGridViewImageColumn = new DataGridViewImageColumn();
                dataGridView1.Columns.Add(dataGridViewImageColumn);
                dataGridViewImageColumn.DefaultCellStyle.NullValue = null;
                dataGridViewImageColumn.Width = 25;
            }
            //dataGridView1.ColumnCount = ColumnCount;
            dataGridView1.RowCount = RowCount;
            // Ширина столбцов
            /*
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].Width = 25;
            }
            */
            // Высота строк
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Height = 25;
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Заполняем границы
            fill_row(0, imageWall, TypeFigure.Wall);
            fill_row(dataGridView1.RowCount-1, imageWall, TypeFigure.Wall);
            fill_column(0, imageWall, TypeFigure.Wall);
            fill_column(dataGridView1.ColumnCount-1, imageWall, TypeFigure.Wall);
            // Заполняем всю таблицу
            Random rnd = new Random();
            for (int i = 1; i < dataGridView1.RowCount - 1; i++)
            {
                fill_row_random(i, rnd);
            }
            if (ActiveBomb.Checked == true)
            {
                imagebomb = Image.FromFile(pathImagebomb);
            }
            else
            {
                imagebomb = null;
            }
            int countBomb = (int)numericUpDown3.Value;
            for (int i = 1; i < dataGridView1.RowCount - 1; i++)
            {
                if(countBomb > 0)
                {
                    fill_Bomb(rnd.Next(1, dataGridView1.ColumnCount - 1), rnd, countBomb);
                    countBomb--;
                }
                
            }
            
            // По умолчанию человек правый нижний угол внутри лабиринта (без границы)
            numCurRowMan = dataGridView1.RowCount - 2;
            numCurColMan = dataGridView1.ColumnCount - 2;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            imageMan = Image.FromFile(pathImageMan);
            imageWall = Image.FromFile(pathImageWall);
            imageTarget = Image.FromFile(pathImageTarget);
            
            labelHour.Text = "";
            labelMin.Text = "";
            labelSec.Text = "";
            

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (radioButton1.Checked)
                {
                    dataGridView1.CurrentCell.Value = null;
                    dataGridView1.CurrentCell.Tag = TypeFigure.Empty;
                }
                else if (radioButton2.Checked)
                {
                    dataGridView1.CurrentCell.Value = imageWall;
                    dataGridView1.CurrentCell.Tag = TypeFigure.Wall;
                }
                else if (radioButton3.Checked)
                {
                    // Человек
                    dataGridView1.CurrentCell.Value = imageMan;
                    dataGridView1.CurrentCell.Tag = TypeFigure.Man;
                    numCurRowMan = e.RowIndex;
                    numCurColMan = e.ColumnIndex;
                }
                else if (radioButton4.Checked)
                {
                    dataGridView1.CurrentCell.Value = imageTarget;
                    dataGridView1.CurrentCell.Tag = TypeFigure.Target;
                }
            }
        }

        private void ControlElem(bool b)
        {
            label1.Enabled = b;
            textBox1.Enabled = b;
            label2.Enabled = b;
            textBox2.Enabled = b;
            label3.Enabled = b;
            numericUpDown1.Enabled = b;
            label4.Enabled = b;
            numericUpDown2.Enabled = b;
            panel1.Enabled = b;
            button1.Enabled = b;
            button2.Enabled = b;
            button3.Enabled = b;
            panel2.Enabled = b;
            numericUpDown3.Enabled = b;
            label7.Enabled = b;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            // Начало игры
            ControlElem(false);
            // Сброс времени
            Sec = 0;
            Min = 0;
            Hour = 0;
            labelSec.Text = "00";
            labelMin.Text = "00";
            labelHour.Text = "00";
            checkBox1.Checked = false;
            dataGridView1.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            timer1.Enabled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Вверх
                case Keys.Up:
                    {
                        if ((TypeFigure)dataGridView1.Rows[numCurRowMan - 1].
                            Cells[numCurColMan].Tag != TypeFigure.Wall)
                        {
                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan].Value = null;
                            dataGridView1.Rows[numCurRowMan - 1].Cells[numCurColMan].Value = imageMan;
                            if ((TypeFigure)dataGridView1.Rows[numCurRowMan - 1].Cells[numCurColMan].Tag == TypeFigure.Target)
                            {
                                timer1.Enabled = false;
                                MessageBox.Show("Вы победили!", "Игра");
                                // Разблокировка элементов
                                ControlElem(true);
                                return;
                            }
                            else if ((TypeFigure)dataGridView1.Rows[numCurRowMan - 1].Cells[numCurColMan].Tag == TypeFigure.Bomb)
                            {
                                timer1.Enabled = false;
                                MessageBox.Show("Вы проиграли", "Игра");
                                // Разблокировка элементов
                                ControlElem(true);
                                return;
                            }

                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan].Tag = TypeFigure.Empty;
                            dataGridView1.Rows[numCurRowMan - 1].Cells[numCurColMan].Tag = TypeFigure.Man;
                            numCurRowMan--;
                        }
                        break;
                    }

                // Вниз
                case Keys.Down:
                    {
                        if ((TypeFigure)dataGridView1.Rows[numCurRowMan + 1].
                            Cells[numCurColMan].Tag != TypeFigure.Wall)
                        {
                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan].Value = null;
                            dataGridView1.Rows[numCurRowMan + 1].Cells[numCurColMan].Value = imageMan;
                            if ((TypeFigure)dataGridView1.Rows[numCurRowMan + 1].Cells[numCurColMan].Tag == TypeFigure.Target)
                            {
                                timer1.Enabled = false;
                                MessageBox.Show("Вы победили!", "Игра");
                                // Разблокировка элементов
                                ControlElem(true);
                                return;
                            }
                            else if ((TypeFigure)dataGridView1.Rows[numCurRowMan + 1].Cells[numCurColMan].Tag == TypeFigure.Bomb)
                            {
                                timer1.Enabled = false;
                                MessageBox.Show("Вы проиграли", "Игра");
                                // Разблокировка элементов
                                ControlElem(true);
                                return;
                            }

                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan].Tag = TypeFigure.Empty;
                            dataGridView1.Rows[numCurRowMan + 1].Cells[numCurColMan].Tag = TypeFigure.Man;
                            numCurRowMan++;
                        }
                        break;
                    }

                // Влево
                case Keys.Left:
                    {
                        if ((TypeFigure)dataGridView1.Rows[numCurRowMan].
                            Cells[numCurColMan - 1].Tag != TypeFigure.Wall)
                        {
                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan].Value = null;
                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan - 1].Value = imageMan;
                            if ((TypeFigure)dataGridView1.Rows[numCurRowMan].Cells[numCurColMan - 1].Tag == TypeFigure.Target)
                            {
                                timer1.Enabled = false;
                                MessageBox.Show("Вы победили!", "Игра");
                                // Разблокировка элементов
                                ControlElem(true);
                                return;
                            }
                            else if ((TypeFigure)dataGridView1.Rows[numCurRowMan].Cells[numCurColMan - 1].Tag == TypeFigure.Bomb)
                            {
                                timer1.Enabled = false;
                                MessageBox.Show("Вы проиграли", "Игра");
                                // Разблокировка элементов
                                ControlElem(true);
                                return;
                            }

                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan].Tag = TypeFigure.Empty;
                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan - 1].Tag = TypeFigure.Man;
                            numCurColMan--;
                        }
                        break;
                    }

                // Вправо
                case Keys.Right:
                    {
                        if ((TypeFigure)dataGridView1.Rows[numCurRowMan].
                            Cells[numCurColMan + 1].Tag != TypeFigure.Wall)
                        {
                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan].Value = null;
                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan + 1].Value = imageMan;
                            if ((TypeFigure)dataGridView1.Rows[numCurRowMan].Cells[numCurColMan + 1].Tag == TypeFigure.Target)
                            {

                                timer1.Enabled = false; 
                                MessageBox.Show("Вы победили!", "Игра");
                                // Разблокировка элементов
                                ControlElem(true);
                                return;
                            }
                            else if ((TypeFigure)dataGridView1.Rows[numCurRowMan].Cells[numCurColMan + 1].Tag == TypeFigure.Bomb)
                            {
                                timer1.Enabled = false;
                                MessageBox.Show("Вы проиграли", "Игра");
                                // Разблокировка элементов
                                ControlElem(true);
                                return;
                            }

                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan].Tag = TypeFigure.Empty;
                            dataGridView1.Rows[numCurRowMan].Cells[numCurColMan + 1].Tag = TypeFigure.Man;
                            numCurColMan++;
                        }
                        break;
                    }
            }
        }

        private string getFormatTime(int t)
        {
            if (t < 10)
            {
                return "0" + t.ToString();
            }
            else
            {
                return t.ToString();
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            Sec++;
            if (Sec == 60)
            {
                Sec = 0;
                Min++;
            }
            if (Min == 60)
            {
                Min = 0;
                Hour++;
            }
            labelHour.Text = getFormatTime(Hour);
            labelMin.Text = getFormatTime(Min);
            labelSec.Text = getFormatTime(Sec);
        }
    }
}