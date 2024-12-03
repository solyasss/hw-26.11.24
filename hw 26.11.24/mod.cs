using System;

namespace hw_26._11._24
{
    public class mod
    {
        public string[,] board { get; private set; }
        public bool player_turn { get; set; }
        public string player_symbol { get; private set; }
        public string computer_symbol { get; private set; }

        public mod()
        {
            board = new string[3, 3];
            player_symbol = "X";
            computer_symbol = "O";
        }

        public void reset()
        {
            board = new string[3, 3];
            player_turn = true;
        }

        public bool check_win(string symbol)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == symbol && board[i, 1] == symbol && board[i, 2] == symbol)
                    return true;
                if (board[0, i] == symbol && board[1, i] == symbol && board[2, i] == symbol)
                    return true;
            }

            if (board[0, 0] == symbol && board[1, 1] == symbol && board[2, 2] == symbol)
                return true;
            if (board[0, 2] == symbol && board[1, 1] == symbol && board[2, 0] == symbol)
                return true;

            return false;
        }

        public bool is_full()
        {
            foreach (var cell in board)
            {
                if (string.IsNullOrEmpty(cell))
                    return false;
            }
            return true;
        }

        public bool make_move(int i, int j, string symbol)
        {
            if (string.IsNullOrEmpty(board[i, j]))
            {
                board[i, j] = symbol;
                return true;
            }
            return false;
        }
    }
}
