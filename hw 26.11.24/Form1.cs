namespace hw_26._11._24
{
    public partial class Form1 : Form
    {
        private bool player_turn;
        private string[,] board = new string[3, 3];
        private Random random = new Random();
        private string player_symbol = "X";
        private string computer_symbol = "O";

        public Form1()
        {
            InitializeComponent();
            new_game();
        }

        private void new_game()
        {
            board = new string[3, 3];

            player_turn = checkbox_first_move.Checked;

            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (c is Button)
                {
                    ((Button)c).Text = "";
                    ((Button)c).Enabled = true;
                }
            }

            if (!player_turn)
            {
                computer_move();
            }
        }

        private void button_new_game_Click(object sender, EventArgs e)
        {
            new_game();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (!player_turn)
                return;

            Button btn = sender as Button;
            if (btn != null && btn.Text == "")
            {
                btn.Text = player_symbol;
                btn.Enabled = false;
                update(btn, player_symbol);
                if (check(player_symbol))
                {
                    MessageBox.Show("You win!");
                    disable_buttons();
                }
                else if (full())
                {
                    MessageBox.Show("Draw!");
                    disable_buttons();
                }
                else
                {
                    player_turn = false;
                    computer_move();
                }
            }
        }

        private void update(Button btn, string symbol)
        {
            int index = int.Parse(btn.Name.Substring(6)) - 1; 
            int i = index / 3;
            int j = index % 3;
            board[i, j] = symbol;
        }

        private bool check(string symbol)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == symbol && board[i, 1] == symbol && board[i, 2] == symbol)
                    return true;
            }

            for (int j = 0; j < 3; j++)
            {
                if (board[0, j] == symbol && board[1, j] == symbol && board[2, j] == symbol)
                    return true;
            }

            if (board[0, 0] == symbol && board[1, 1] == symbol && board[2, 2] == symbol)
                return true;

            if (board[0, 2] == symbol && board[1, 1] == symbol && board[2, 0] == symbol)
                return true;

            return false;
        }

        private bool full()
        {
            foreach (var cell in board)
            {
                if (cell == null)
                    return false;
            }
            return true;
        }

        private void disable_buttons()
        {
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (c is Button)
                {
                    ((Button)c).Enabled = false;
                }
            }
        }

        private void computer_move()
        {
            if (player_turn)
                return;

            int level = hard_level();

            switch (level)
            {
                case 1:
                    easy_computer();
                    break;
                case 2:
                    medium_computer();
                    break;
                case 3:
                    hard_computer();
                    break;
            }

            if (check(computer_symbol))
            {
                MessageBox.Show("You lose!");
                disable_buttons();
            }
            else if (full())
            {
                MessageBox.Show("Draw!");
                disable_buttons();
            }
            else
            {
                player_turn = true;
            }
        }

        private int hard_level()
        {
            if (radio_easy.Checked)
                return 1;
            else if (radio_medium.Checked)
                return 2;
            else
                return 3;
        }

        private void easy_computer()
        {
            while (true)
            {
                int i = random.Next(0, 3);
                int j = random.Next(0, 3);
                if (board[i, j] == null)
                {
                    move(i, j, computer_symbol);
                    break;
                }
            }
        }

        private void medium_computer()
        {
            if (!chance(computer_symbol))
            {
                if (!chance(player_symbol))
                {
                    easy_computer();
                }
            }
        }

        private void hard_computer()
        {
            medium_computer();
        }

        private bool chance(string symbol)
        {
            for (int i = 0; i < 3; i++)
            {
                int count = 0, empty_j = -1;
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == symbol)
                        count++;
                    else if (board[i, j] == null)
                        empty_j = j;
                }
                if (count == 2 && empty_j != -1)
                {
                    move(i, empty_j, computer_symbol);
                    return true;
                }
            }

            for (int j = 0; j < 3; j++)
            {
                int count = 0, empty_i = -1;
                for (int i = 0; i < 3; i++)
                {
                    if (board[i, j] == symbol)
                        count++;
                    else if (board[i, j] == null)
                        empty_i = i;
                }
                if (count == 2 && empty_i != -1)
                {
                    move(empty_i, j, computer_symbol);
                    return true;
                }
            }

            int count_diag1 = 0, empty_diag1 = -1;
            for (int i = 0; i < 3; i++)
            {
                if (board[i, i] == symbol)
                    count_diag1++;
                else if (board[i, i] == null)
                    empty_diag1 = i;
            }
            if (count_diag1 == 2 && empty_diag1 != -1)
            {
                move(empty_diag1, empty_diag1, computer_symbol);
                return true;
            }

            int count_diag2 = 0, empty_diag2_i = -1, empty_diag2_j = -1;
            for (int i = 0; i < 3; i++)
            {
                int j = 2 - i;
                if (board[i, j] == symbol)
                    count_diag2++;
                else if (board[i, j] == null)
                {
                    empty_diag2_i = i;
                    empty_diag2_j = j;
                }
            }
            if (count_diag2 == 2 && empty_diag2_i != -1)
            {
               move(empty_diag2_i, empty_diag2_j, computer_symbol);
                return true;
            }

            return false;
        }

        private void move(int i, int j, string symbol)
        {
            board[i, j] = symbol;
            int index = i * 3 + j + 1;
            string button_name = $"button{index}";
            Button btn = this.Controls.Find(button_name, true)[0] as Button;
            btn.Text = symbol;
            btn.Enabled = false;
        }
    }
}
