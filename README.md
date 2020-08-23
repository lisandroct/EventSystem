# Event System
Event system for Unity using ScriptableObjects based on the concept introduced by Ryan Hipple in his [talk](https://www.youtube.com/watch?v=raQ3iHhE_Kk) at Unite 2017.

## Installation
This package depends on [Core](https://github.com/lisandroct/Core). Both packages can be installed through the Unity Package Manager or by modifying the project packages manifest manually.
```
{
    "dependencies": {
        "com.lisandroct.core": "https://github.com/lisandroct/Core.git",
        "com.lisandroct.events": "https://github.com/lisandroct/EventSystem.git"
    }
}
```

## Usage
After installing **Event System**, the settings can be found in `Edit/Project Settings/Event System`. By default, the settings get generated with a few basic types for your convenience.

### Code generation
Before being able to use **Event System**, it's necessary to generate the required code for the different event types. In order to do so, under `Event Types` in settings, click `Generate` and all the necessary code will be generated automatically for you.

### Adding new event types
Each event can have up to four arguments. You can add new event types under `Add New` in settings.

To add a new type argument, search for it by filtering by namespace and/or type name and click on it. If you want to remove a previously added type, just click on it. 

The event type will be automatically assigned a name from its argument types. The name can be changed but it's important to not use a name previously used.

### Events
After generating the code, you can create event assets for every event type. Just right click in the Project Window and you'll see all the possible options under `Events`. The event will be automatically named `On[EventName]Event` but can be renamed.

In the Unity Editor you can just drag and drop the reference to the new event like you do with any other asset type.

In order to raise the event, every event has a `Raise` method.

### Test events
One of the big benefits of using **Event System** over other solutions is that you can raise and test the events by selecting the asset in the Project Window in runtime.

#### Runtime events
Since events are just ScriptableObjects, it's super easy to create new events on runtime by just calling `ScriptableObject.CreateInstance<[EventType]>()` or `ScriptableObject.CreateInstance(typeof([EventType]))`.

### Listeners
There's nothing to prevent you from subscribing to events from code (by using the `Register` and `Unregister` methods) but **Event System** also provides you with Listeners for each event type you generate.

The listeners expect the reference to the event and call a UnityEvent when the event is raised.

## Example
If this is your first time using the package, it's recommended to import the example from the Unity Package Manager. Note that the example contains the definition and code for Bool and Color events, in order to avoid conflicts, make sure to import the example before generating any **Event System** code by yourself.

## Important Note
**Event System** relies on the files saved and generated under `lisandroct` folder, it's important to not rename or modify any subfolder or file since everything was automatically generated and the whole system relies on it.