namespace hw_26._11._24
{
    public interface i_view
    {
        void update_button(int index, string symbol);
        void enable_buttons(bool enabled);
        void show_message(string message);
        void reset_board();
        void set_button_enabled(int index, bool enabled);

        bool is_player_first { get; }
        int selected_level { get; }

        event EventHandler<int> cell_clicked;
        event EventHandler new_game_clicked;
    }
}