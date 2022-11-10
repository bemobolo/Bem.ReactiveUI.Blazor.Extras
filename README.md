# Bem.ReactiveUI.Blazor.Extras
![Nuget (with prereleases)](https://img.shields.io/nuget/v/Bem.ReactiveUI.Blazor.Extras?logo=nuget)
![NuGet Downloads](https://img.shields.io/nuget/dt/Bem.ReactiveUI.Blazor.Extras?logo=nuget)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/bemobolo/Bem.ReactiveUI.Blazor.Extras?logo=github)
![Azure DevOps builds](https://img.shields.io/azure-devops/build/dbalogh/Bem.ReactiveUI.Blazor.Extras/18/main?logo=azurepipelines)
![Azure DevOps tests](http://bemobolo.ddns.net/azure-devops/tests/dbalogh/Bem.ReactiveUI.Blazor.Extras/18/main?compact_message&logo=azuredevops)
![Azure DevOps coverage](http://bemobolo.ddns.net/azure-devops/coverage/dbalogh/Bem.ReactiveUI.Blazor.Extras/18/main?logo=azuredevops)
![License](https://img.shields.io/badge/license-GPLv3-blue?logo=googledocs&logoColor=f5f5f5)
# Overview
This library extends [ReactiveUI.Blazor](https://github.com/reactiveui/ReactiveUI/tree/main/src/ReactiveUI.Blazor) and helps using [ReactiveUI](https://github.com/reactiveui/ReactiveUI) with [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) more comfortable and more competitive by introducing rendering performance optimizations.

## Installation
Install the library from [nuget](https://www.nuget.org/packages/Bem.ReactiveUI.Blazor.Extras) via  

```dotnet add package Bem.ReactiveUI.Blazor.Extras```
## Usage
Inherit your component from ```AdvancedComponentBase<TViewModel>```:  
```html
@inherit AdvancedComponentBase<MyViewModel>
```
Then subscribe to change notifications of ViewModels’ (or its descendants') properties using
```AdvancedComponentBase<TViewModel>.From``` method:
```html
<SomeComponent Text="@From(x => x.ViewModel.Child.Property)" />
```
For additional options and use cases see [Features](#features) section.
## Motivation
In real-life scenarios developers face the fact that as applications evolve they get more and more complex. The number of components of the user interface increase, the visual tree/render tree becomes larger, the management of app state and dependencies cause pain.  
If the applied framework is designed and used well and the underlying platform and its resources are managed well these don't cause serious headaches. The application stays fast and responsible and provides a good user experience which is the ultimate goal after all.
Unfortunately the platform and the underlying stack of a web app framework is quite robust: it includes tons of technologies, protocols, patterns, multiple layers of abstractions, etc.  
Because of this developers are forced to manage available resources even more carefully, otherwise the overhead of the many abstraction layers makes the application unusable.
And here comes a possible issue you may face when using Blazor:  

Though it's super easy to build a fully functional complex SPA it is also super easy to build an unresponsive one that consumes too much resources. Why? It's because of its design, especially the [lifecyle of the components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle).  
Microsoft dedicates a whole [page for performance optimization](https://learn.microsoft.com/en-us/aspnet/core/blazor/performance) as slow rendering is a ~~known issue~~ common side effect.  

**So the main purpose of this library is providing better performance and UX by reducing redundant renderings of Blazor component subtress while still using the goodies of reactive programming with ReactiveUI.**

### Why ReactiveUI, why MVVM?
MVVM is a proven approach, a de-facto industry standard for UI state management. With reactive programming it makes managing data flow and application state simple, consequent and maintainable. Once you get familiar with MVVM pattern and reactive programming with its functional approach you will stick to them forever.
## How it Works  
Extras library provides a custom base component called ```AdvancedBaseComponent<TViewModel>``` which alters the standard Blazor component lifecycle in the following way:  

<a href="https://github.com/bemobolo/Bem.ReactiveUI.Blazor.Extras/blob/main/img/Advanced%20Component%20Lifecycle.png?raw=true" tagret="_blank"><img src="https://github.com/bemobolo/Bem.ReactiveUI.Blazor.Extras/blob/main/img/Advanced%20Component%20Lifecycle.png" width="480" alt="Advanced Component Render"/></a>

First of all it uses the notification capabilities of ViewModel instances which implements ```INotifyPropertyChanged``` and helps subscribing - even inline in razor syntax - to property chains to observe model state changes. Once a change notification arrives, the component becomes subject to rendering.
Second, when ```SetParametersAsync``` called an advanced component parameter change detection algorithm kicks in.  

The default change detection in Blazor considers parameters of complex objects (reference types) possibly changed - since there is no way to decide if their properties and/or child objects have been actually mutated or not. There is no data of previou state to compare to and/or it would be very costly.  

The Extras library on the other hand is able to detect immutable types and even graphs of immutable types. Values of these types are considered unchanged during render.  
Moreover if a type implements ```INotifyPropertyChanged``` interface the advanced algorithm indirectly delegates the task to the developer to decide if the object mutated or not. That is: the developer must create bindings/subscriptions to objects implementing ```INotifyPropertyChanged``. As a result these bindings will automatically notify the component about the mutation and the required rendering.

## Features
### Advanced Component Creation
#### With Inheritance
Inherit your component from ```AdvancedComponentBase<TViewModel>```:  
```html
@inherit AdvancedComponentBase<MyViewModel>
```
#### With Decoration
Inherit from an existing component and decorate with ```[AdvancedComponentAttribute]```. The library uses a source generator looking for components decorated with the above attribute. It creates a partial class implementation similar to ```AdvancedComponentBase<TViewModel>``` class.   
Optionally add a type parameter if required:  
```html
@attribute [AdvancedComponentAttribute]
@typeparam TItem
@inherit SomeThirdPartyComponent<TItem>
```
### Inline Binding in Razor File
Here in the context of Extras library binding is a bit different than a standard Blazor one. It is a subscription to ViewModel property (chain) change and not just a property expression supplying value for bound component parameters.  
With the help of the Extras library bindings' lifecycle can be automatically managed. Since the ```BuildRenderTree``` method is generated from the component's razor file the bindings defined in razor follow the lifecycle of render tree items.
#### One-way binding
Use ```From(Expression propertyChainExpression)``` method in razor to bind ViewModel property to a parameter of a component:
```html
<SomeComponent Property="@From(x => x.ViewModel.SomeProperty)" />
```
#### Two-way binding
Achieving two way binding is an expression when using ```@bind-Value``` directive. No method calls or such is allowed.  
#### Subscription to Collection Changes
Note that subscription to collection properties also observes the changes of the collection items if possible - if and only if collection implements ```INotifyCollectionChanged``` and items implement ```INotifPropertyChanged``` interfaces:
```html
<Repeater Items="@From(x => x.ViewModel.ObservableCollection)" Context="item">
  @item.Property
</Repeater>
```
##### Explicit subscription 
Use ```@bind``` directive and insert an additional attribute ensuring the subscription:
```html
<TextEdit @bind-Text="@ViewModel.Name" subscribe="@To(x => x.ViewModel.Name)"/>
```
The ```subscribe``` attribute is an ad-hoc one here there is no special meaning or convention behind it or any logic implemented. The only purpose of it is that it's legal (source code generator won't complain about it) and the attribute along with  the ```To``` method call will be present in the generated render tree.  
Also note that ```To``` method call always returns ```null```. It does not provide the value of the bound property like the ```From``` method does. Its only purpose is to ensure the binding creation. Also by returning ```null``` we can make sure ```subscribe``` attribute will have no HTML output at all.
##### Splitting up @bind-Value
Use the ```From``` method of the advanced component and 
```html
<TextEdit Text="@From(x => x.ViewModel.Name)" TextChanged="@(UpdateNameAsync)"/>
@code 
{
    private Task UpdateNameAsync(string name) => ViewModel.Name = name;
}
```
### Performance Optimization
As stated before the Blazor render approach often results in unnecessary (and costly) component subtree renders. One of the aims of this library is preventing these redundant renders. This is achieved by listening to changes of ViewModels of components and only allowing component rendering if a change is detected.  
Important to mention here is that **event callbacks of advanced components won't call ```StateHasChanged``` automatically!** It can be turned on though by setting ```ForceRenderOnEvent``` property to ```true```.
### Advanced Change Detection
Extras library is able to detect immutable types and even graphs of immutable types. Values of these types are considered unchanged during render.  
Moreover if a type of a parameter implements ```INotifyPropertyChanged``` interface the advanced algorithm expects the use of the notification mechanism by the developer by creating bindings. A binding can then notify the component about the state change and the required render.  
Further types are subject to advanced change detection. If they are used in parameters the library can check if objects of these types changed or not and then renders/skips render according to it:  
- types decorated with ```[ImmutableObjectAttribute(immutable: true)]```
- types implementing ```IEquatable<T>```
- primitive types
- specific value types: ```decimal```, ```DateTime```, ```DateTimeOffset```, ```Guid```, ```TimeSpan``` 
- ```Uri```, ```string```, ```Type``` classes
- ```Lookup<T>```, ```Immutable*``` collections of the above types
- ```Nullable<T>``` versions of the above types
## When to Use
If you have a complex ReactiveUI based app already it's worth introducing the Extras library. If you experience slow rendering speed due to high number of components and unnecessary rerendering of component subtrees even without any actual state changes or you just need an easy way of declaring and maintaining data bindings this tool is for you!
Also if you start a new app and would like to make it nice, clean and fast use ReactiveUI together with Extras.
## When not to Use
For simple applications, applying the Extras library might not be worth it. If one or more of the below circumstances applies I don't recommend to use the lib:
- app does not use ReactiveUI or not reactive oriented at all
- app involves small amount of components
- the component hierarchy is not too complex 
- component parameters are of only primitive types
In such situations the standard design may give good overall performance.
# Troubleshooting
## Component Rendering Skipped
As written earlier a component needs proper subscriptions to ViewModel changes in order to refresh.  
If you experience that a component is stuck double check if one- or two-way bindings are correctly setup and not missing.
## Component Renders Multiple Times
Bindings are distinct by component instance and property expression. This means if you subscribe multiple times from the same component to exactly the same property using exactly the same property chain the library detects it and does not subscribe multiple times.  
However sometimes it's possible to subscribe to the same property using a different property chain:
```html
// No problem here, the component which owns this render tree will not be rendered multiple times
<Component Property="@From(x => x.ViewModel.SomeProperty)" />
<AnotherComponent AnotherProperty="@From(x => x.ViewModel.SomeProperty)" />
```
```html
// These subscriptions are not the same! Though the same property is bound, the property chain expression differs!
<Component Property="@From(x => x.ViewModel.SomeProperty)" />
<AnotherComponent AnotherProperty="@From(x => x.ViewModel.Parent.ViewModel.SomeProperty)" />
```
# Demo App
The repository includes a sample web application demonstrating the advantages of the library. The SPA is implemented in two ways: one with standard approach and second with the Extras library.  
The below screen captures are from the demo app and they show the enhancement of the whole user experience by reducing render times and render counts. The first animation shows how subtrees of standard components with complex ViewModels are rerendered if parent changes:  
<a href="https://github.com/bemobolo/Bem.ReactiveUI.Blazor.Extras/blob/main/img/reactive.webm?raw=true" tagret="_blank"><img src="https://github.com/bemobolo/Bem.ReactiveUI.Blazor.Extras/blob/main/img/reactive.gif" width="480" alt="Advanced Component Render"/></a>  
Versus the advanced Component, which prevents rerendering subtrees if not changed even if parameter is complex:  
<a href="https://github.com/bemobolo/Bem.ReactiveUI.Blazor.Extras/blob/main/img/extra.webm?raw=true" tagret="_blank"><img src="https://github.com/bemobolo/Bem.ReactiveUI.Blazor.Extras/blob/main/img/extra.gif" width="480" alt="Advanced Component Render"/></a>  
The delayed red flashes of the background shows when the HTML DOM changes because of component rendering. Note how subtree renders skipped and render counts and render times differ among the two pages.
