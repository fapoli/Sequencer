namespace MoodyLib.Sequencer {
    /// <summary>
    /// The context that is passed to each action in the sequence.
    /// </summary>
    public class SequenceContext {
        private readonly Sequence _sequence;
    
        public SequenceContext(Sequence sequence) {
            _sequence = sequence;
        }
        
        /// <summary>
        /// Moves the cursor to the next action in the sequence.
        /// </summary>
        public void Next() {
            _sequence.Next();
        }
    }
}