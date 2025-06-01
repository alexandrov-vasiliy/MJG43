using _Game.Bets;
using _Game.Deck;

namespace _Game
{
    public static class G
    {
        public static Main main;

        public static Hand playerHand;
        
        public static Hand enemyHand;

        public static Board board;

        public static Deck.Deck deck;

        public static CoreLoop coreLoop;

        public static Brush brush;
        
        public static UI ui;

        public static DeckShuffler deckShuffler;

        public static BetSystem betSystem;

        public static CameraSwitcher cameraSwitcher;

        public static Feel feel;
    }
}