using System;
using System.Linq;
using System.Windows.Forms;

namespace hw_26._11._24
{
    public class presenter
    {
        private readonly i_krestiki_noliki_view view;
        private readonly krestiki_noliki_model model;
        private readonly Random random;

        public presenter(i_krestiki_noliki_view view)
        {
            this.view = view;
            model = new krestiki_noliki_model();
            random = new Random();

            this.view.cell_clicked += on_cell_clicked;
            this.view.new_game_clicked += on_new_game;
        }

        public void initialize()
        {
            start_new_game();
        }

        private void start_new_game()
        {
            model.reset();
            view.reset_board();
            view.enable_buttons(true);
            if (view.is_player_first)
            {
                model.player_turn = true;
            }
            else
            {
                model.player_turn = false;
                computer_move();
            }
        }

        private void on_new_game(object sender, EventArgs e)
        {
            start_new_game();
        }

        private void on_cell_clicked(object sender, int index)
        {
            if (!model.player_turn)
                return;

            int i = index / 3;
            int j = index % 3;

            if (model.make_move(i, j, model.player_symbol))
            {
                view.update_button(index, model.player_symbol);
                view.set_button_enabled(index, false);

                if (model.check_win(model.player_symbol))
                {
                    view.show_message("You won!");
                    view.enable_buttons(false);
                }
                else if (model.is_full())
                {
                    view.show_message("Draw!");
                    view.enable_buttons(false);
                }
                else
                {
                    model.player_turn = false;
                    computer_move();
                }
            }
        }

        private void computer_move()
        {
            int level = view.selected_level;
            bool move_made = false;

            switch (level)
            {
                case 1:
                    move_made = easy_computer_move();
                    break;
                case 2:
                    move_made = medium_computer_move();
                    break;
                case 3:
                    move_made = hard_computer_move();
                    break;
                default:
                    move_made = easy_computer_move();
                    break;
            }

            if (move_made)
            {
                if (model.check_win(model.computer_symbol))
                {
                    view.show_message("Computer won!");
                    view.enable_buttons(false);
                }
                else if (model.is_full())
                {
                    view.show_message("Draw!");
                    view.enable_buttons(false);
                }
                else
                {
                    model.player_turn = true;
                }
            }
            else
            {
                view.show_message("Draw!");
                view.enable_buttons(false);
            }
        }

        private bool easy_computer_move()
        {
            var empty_cells = get_empty_cells();
            if (empty_cells.Any())
            {
                var cell = empty_cells[random.Next(empty_cells.Count)];
                return make_computer_move(cell.Item1, cell.Item2);
            }
            return false;
        }

        private bool medium_computer_move()
        {
            if (try_chance(model.computer_symbol))
                return true;
            if (try_chance(model.player_symbol))
                return true;
            return easy_computer_move();
        }

        private bool hard_computer_move()
        {
            return medium_computer_move();
        }

        private bool try_chance(string symbol)
        {
            for (int i = 0; i < 3; i++)
            {
                int count = 0;
                int empty_j = -1;
                for (int j = 0; j < 3; j++)
                {
                    if (model.board[i, j] == symbol)
                        count++;
                    else if (string.IsNullOrEmpty(model.board[i, j]))
                        empty_j = j;
                }
                if (count == 2 && empty_j != -1)
                {
                    return make_computer_move(i, empty_j);
                }

                count = 0;
                int empty_i = -1;
                for (int j = 0; j < 3; j++)
                {
                    if (model.board[j, i] == symbol)
                        count++;
                    else if (string.IsNullOrEmpty(model.board[j, i]))
                        empty_i = j;
                }
                if (count == 2 && empty_i != -1)
                {
                    return make_computer_move(empty_i, i);
                }
            }

            int diag_count = 0;
            int empty_diag = -1;
            for (int i = 0; i < 3; i++)
            {
                if (model.board[i, i] == symbol)
                    diag_count++;
                else if (string.IsNullOrEmpty(model.board[i, i]))
                    empty_diag = i;
            }
            if (diag_count == 2 && empty_diag != -1)
            {
                return make_computer_move(empty_diag, empty_diag);
            }

            diag_count = 0;
            empty_diag = -1;
            int empty_diag_j = -1;
            for (int i = 0; i < 3; i++)
            {
                int j = 2 - i;
                if (model.board[i, j] == symbol)
                    diag_count++;
                else if (string.IsNullOrEmpty(model.board[i, j]))
                {
                    empty_diag = i;
                    empty_diag_j = j;
                }
            }
            if (diag_count == 2 && empty_diag != -1)
            {
                return make_computer_move(empty_diag, empty_diag_j);
            }

            return false;
        }

        private bool make_computer_move(int i, int j)
        {
            if (model.make_move(i, j, model.computer_symbol))
            {
                int index = i * 3 + j;
                view.update_button(index, model.computer_symbol);
                view.set_button_enabled(index, false);
                return true;
            }
            return false;
        }

        private System.Collections.Generic.List<(int, int)> get_empty_cells()
        {
            var list = new System.Collections.Generic.List<(int, int)>();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (string.IsNullOrEmpty(model.board[i, j]))
                        list.Add((i, j));
            return list;
        }
    }
}
