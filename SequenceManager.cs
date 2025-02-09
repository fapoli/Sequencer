using System.Collections.Generic;
using UnityEngine;

namespace MoodyLib.Sequencer {
    
    /// <summary>
    /// The SequenceManager class is responsible for managing all Sequence objects in the game.
    /// </summary>
    public class SequenceManager : MonoBehaviour {
        public static SequenceManager instance;
        
        private List<Sequence> _sequences = new();
        
        private void Awake() {
            instance = this;
        }
        
        private void Update() {
            _sequences.ForEach(sequence => sequence.Update());
            _sequences.RemoveAll(sequence => sequence.isComplete);
        }
        
        internal void AddSequence(Sequence sequence) {
            _sequences.Add(sequence);
        }
    }
}