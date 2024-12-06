using System;
using System.Windows.Forms;

namespace hw_26._11._24
{
    public partial class Form1 : Form, i_view
    {
        private Button[] buttons;

        public event EventHandler<int> cell_clicked = delegate { };
        public event EventHandler new_game_clicked = delegate { };

        private presenter presenter;

        public Form1()
        {
            InitializeComponent();

            buttons = new Button[9] { button1, button2, button3, button4, button5, button6, button7, button8, button9 };

            foreach (var btn in buttons)
                btn.Click += on_button_click;

            button_new_game.Click += on_new_game_click;

            presenter = new presenter(this);
            presenter.initialize();
        }

        private void on_button_click(object? sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                int index = Array.IndexOf(buttons, btn);
                if (index >= 0)
                {
                    cell_clicked(this, index);
                }
            }
        }

        private void on_new_game_click(object? sender, EventArgs e)
        {
            new_game_clicked(this, EventArgs.Empty);
        }

        public void update_button(int index, string symbol)
        {
            buttons[index].Text = symbol;
        }

        public void enable_buttons(bool enabled)
        {
            foreach (var btn in buttons)
                btn.Enabled = enabled;
        }

        public void show_message(string message)
        {
            MessageBox.Show(message);
        }

        public void reset_board()
        {
            foreach (var btn in buttons)
            {
                btn.Text = "";
                btn.Enabled = true;
            }
        }

        public void set_button_enabled(int index, bool enabled)
        {
            buttons[index].Enabled = enabled;
        }

        public bool is_player_first
        {
            get { return checkbox_first_move.Checked; }
        }

        public int selected_level
        {
            get
            {
                if (radio_easy.Checked)
                    return 1;
                if (radio_medium.Checked)
                    return 2;
                return 3;
            }
        }
    }
}
