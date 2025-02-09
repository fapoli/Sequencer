using System;
using UnityEngine;

namespace MoodyLib.Sequencer {
    
    /// <summary>
    /// A collection of preset actions that can be used in a Sequence.
    /// </summary>
    public static class Actions {
        
        /// <summary>
        /// Moves the transform to the target position at the given speed.
        /// </summary>
        /// <param name="transform">The transform that will be moved.</param>
        /// <param name="targetPosition">The target position to which the transform will be moved.</param>
        /// <param name="speed">The speed at which the transform will move.</param>
        /// <returns>The action that can be passed to the Sequence.</returns>
        public static Action<SequenceContext> MoveTo(Transform transform, Vector3 targetPosition, float speed) {
            return context => {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
                
                if (transform.position == targetPosition) {
                    context.Next();
                }
            };
        }

        /// <summary>
        /// Rotates the transform to the target rotation at the given speed.
        /// </summary>
        /// <param name="transform">The transform that will be rotated.</param>
        /// <param name="targetRotation">The target rotation to which the transform will be rotated.</param>
        /// <param name="speed">The speed at which the transform will rotate.</param>
        /// <returns>The action that can be passed to the Sequence.</returns>
        public static Action<SequenceContext> RotateTo(Transform transform, Quaternion targetRotation, float speed) {
            return context => {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * speed);
                
                if (transform.rotation == targetRotation) {
                    context.Next();
                }
            };
        }
        
        /// <summary>
        /// Scales the transform to the target scale at the given speed.
        /// </summary>
        /// <param name="transform">The transform that will be scaled.</param>
        /// <param name="targetScale">The target scale to which the transform will be scaled.</param>
        /// <param name="speed">The speed at which the transform will scale.</param>
        /// <returns>The action that can be passed to the Sequence.</returns>
        public static Action<SequenceContext> ScaleTo(Transform transform, Vector3 targetScale, float speed) {
            return context => {
                transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, Time.deltaTime * speed);
                
                if (transform.localScale == targetScale) {
                    context.Next();
                }
            };
        }
        
        /// <summary>
        /// Delays the sequence for the given duration.
        /// </summary>
        /// <param name="duration">The duration of the delay.</param>
        /// <returns>The action that can be passed to the Sequence.</returns>
        public static Action<SequenceContext> Delay(float duration) {
            var timer = 0f;
            return context => {
                timer += Time.deltaTime;
                if (timer >= duration) {
                    context.Next();
                }
            };
        }
    }
}