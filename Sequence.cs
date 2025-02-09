using System;
using System.Collections.Generic;
using MoodyLib.Sequencer;
using UnityEngine;

namespace MoodyLib.Sequencer {
    
    /// <summary>
    /// A Sequence of actions that will be executed in order, one after the other. Useful for chaining animations and other actions.
    /// </summary>
    public class Sequence {
        public bool isComplete => _cursor >= _actions.Count;

        private List<Action<SequenceContext>> _actions = new();
        
        private int _cursor = 0;
        private SequenceContext _context;

        private Sequence() {
            _context = new SequenceContext(this);
        }
        
        /// <summary>
        /// Creates a new Sequence object with the given actions.
        /// </summary>
        /// <param name="action">The initial actions to add to the sequence. You need to manually call context.Next() in this actions, in order to advance the sequence.</param>
        public static Sequence Of(params Action<SequenceContext>[] actions) {
            return  new Sequence().AndThen(actions);;
        }
        
        /// <summary>
        /// Creates a new Sequence object with the given actions.
        /// </summary>
        /// <param name="action">The initial actions to add to the sequence.</param>
        public static Sequence Of(params Action[] actions) {
            return  new Sequence().AndThen(actions);
        }
        
        /// <summary>
        /// Creates a new Sequence object with a delay action at the beginning.
        /// </summary>
        public static Sequence Wait(float duration) {
            return Of(Actions.Delay(duration));
        }
        
        /// <summary>
        /// Creates a new Sequence by joining with a list of sequences, and waits for them to complete before continuing.
        /// </summary>
        /// <param name="sequences">The sequences to join with.</param>
        public static Sequence Join(params Sequence[] sequences) {
            return new Sequence().AndJoinWith(sequences);
        }
        
        /// <summary>
        /// Adds actions to the sequence.
        /// </summary>
        /// <param name="actions">The actions to add to the sequence. You need to manually call context.Next() in order to advance the sequence.</param>
        public Sequence AndThen(params Action<SequenceContext>[] actions) {
            if (actions.Length == 1) {
                _actions.Add(actions[0]);
                return this;
            }

            var sequences = new Sequence[actions.Length];
            for (var i = 0; i < actions.Length; i++) {
                sequences[i] = Sequence.Of(actions[i]);
            }
            
            return AndJoinWith(sequences);
        }

        /// <summary>
        /// Adds actions to the sequence.
        /// </summary>
        /// <param name="action">The actions to add to the sequence.</param>
        public Sequence AndThen(params Action[] actions) {
            if (actions.Length == 1) {
                _actions.Add((context) => {
                    actions[0].Invoke();
                    context.Next();
                });
                return this;
            }
            
            var sequences = new Sequence[actions.Length];
            for (var i = 0; i < actions.Length; i++) {
                sequences[i] = Sequence.Of(actions[i]);
            }
            
            return AndJoinWith(sequences);
        }
        
        /// <summary>
        /// Adds a delay action to the sequence.
        /// </summary>
        /// <param name="duration">The duration of the delay.</param>
        public Sequence AndWait(float duration) {
            return AndThen(Actions.Delay(duration));
        }
        
        /// <summary>
        /// Runs a list of sequences in parallel, and waits for them to complete before continuing.
        /// </summary>
        /// <param name="sequences">The sequences to run in parallel.</param>
        public Sequence AndJoinWith(params Sequence[] sequences) {

            AndThen((context) => {
                foreach (var sequence in sequences) {
                    if(!sequence.isComplete) return;
                }
                
                Debug.Log("All sequences are complete");
                context.Next();
            });
            
            foreach (var sequence in sequences) {
                sequence.Run();
            }
            
            return this;
        }
        

        /// <summary>
        /// Adds actions to the sequence, if the given condition is true.
        /// </summary>
        /// <param name="condition">A condition that has to be true in order for the actions to be passed to the sequence.</param>
        /// <param name="actions">The actions that will be added to the sequence in case the condition is true. You need to manually call context.Next() in order to advance the sequence.</param>
        /// <returns></returns>
        public Sequence AndThenIf(Func<bool> condition, params Action<SequenceContext>[] actions) {
            if (condition.Invoke()) {
                AndThen(actions);
            }
                
            return this;
        }
            
        /// <summary>
        /// Adds actions to the sequence, if the given condition is true.
        /// </summary>
        /// <param name="condition">A condition that has to be true in order for the actions to be passed to the sequence.</param>
        /// <param name="actions">The actions that will be added to the sequence in case the condition is true.</param>
        public Sequence AndThenIf(Func<bool> condition, params Action[] actions) {
            if (condition.Invoke()) {
                AndThen(actions);
            }
                
            return this;
        }
        
        /// <summary>
        /// Runs the sequence, by adding it to the SequenceManager.
        /// </summary>
        public void Run() {
            SequenceManager.instance.AddSequence(this);
        }
        
        internal void Next() {
            _cursor++;
        }

        internal void Update() {
            if (isComplete) return;
            _actions[_cursor].Invoke(_context);
        }
    }
}



