# Sequencer

A component that allows playing a sequence of actions, one after the other. Useful for executing a set of animations. The library includes some basic example methods, but you can create your own to extend the functionality.

## Contents
- **Sequence.cs**: This class represents the sequence of actions that will be executed.
- **SequenceContext.cs**: An action is essentially a method that receives this SequenceContext class. This class allows the actions to communicate with the sequence, in order to notify when an action ends.
- **Actions.cs**: A static class that contains some basic actions that can be passed to the sequences.
- **SequenceManager.cs**: A MonoBehaviour that manages the execution of all the sequences. 
- **Sequencer.prefab**: A prefab that has to be placed in the scene, and contains the SequenceManager component.

## Why would you want to use this?
The behaviour that this package enables can already be achieved by using Unity's built-in coroutines. However, this package provides a more structured way of creating sequences, and allows you to create custom actions that can be reused in different sequences. This can be useful when you have a lot of sequences that need to be executed in a specific order, and you want to keep the code clean and organized.

## How to use
1. Place the Sequencer prefab inside your Unity scene
2. You can now create sequences by using the Sequence builder methods.

## Example usage of the Sequence builder
```csharp
Sequence.Of(_audioSource.Play(_someAudioClip))
    .AndThen(Actions.MoveTo(transform, new Vector3(0, 0, 0), 500))
    .AndThenIf(()=> shouldDestroy, () => Destroy(gameObject))
    .Run();
```

or...

```csharp
Sequence.Wait(delay)
    .AndThen(() => _audioSource.Play(_someAudioClip))
    .AndThen(Actions.MoveTo(transform, new Vector3(0, 0, 0), 500))
    .AndThenIf(()=> shouldDestroy, () => Destroy(gameObject))
    .Run();
```

Multiple actions can be also sent at once to the `Of` or the `AndThen` methods. This will run the actions in parallel, and continue with the sequence when all these actions have completed.

```csharp
Sequence.Wait(delay)
    .AndThen(
        Actions.MoveTo(transform, new Vector3(0, 0, 0), 500), 
        Actions.ScaleTo(transform, new Vector3(1.1f, 1.1f, 1.1f), 1f))
    .AndThenIf(()=> shouldDestroy, () => Destroy(gameObject))
    .Run();
```

## Joining sequences
You can join sequences by using the `Join` method. This will execute the sequences in parallel, and will continue with the next action when all the sequences have finished.

```csharp
var moveSequence = Sequence.Wait(delay)
    .AndThen(Actions.MoveTo(transform, new Vector3(0, 0, 0), 500));

var scaleSequence = Sequence.Wait(delay)
    .AndThen(Actions.ScaleTo(transform, new Vector3(1.1f, 1.1f, 1.1f), 1f));

Sequence.Join(moveSequence, scaleSequence)
    .AndThen(() => Debug.Log("Both sequences have finished!"))
    .AndThenIf(()=> shouldDestroy, () => Destroy(gameObject))
    .Run();
```
An `AndThenJoin` method is also provided if case this behaviour is required in the middle of a sequence.


## Example implementation of a custom action
```csharp
// If this Delay method is passed into a Sequencer, it will run in the update method, 
// until the context.Next() method is called. When this happens, the next action in 
// the sequence will be executed.
public static Action<SequenceContext> Delay(float duration) {
    var timer = 0f;
    return context => {
        timer += Time.deltaTime;
        if (timer >= duration) {
            context.Next();
        }
    };
}
```

Check the documentation inside the corresponding classes to learn about the different parameters you can send to the methods described above.
