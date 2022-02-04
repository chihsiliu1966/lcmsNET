# ProfileSequenceDescriptor Class

Namespace: lcmsNET  
Assembly: lcmsNET.dll

Represents a profile sequence descriptor. This class cannot be inherited.

```csharp
public sealed class ProfileSequenceDescriptor : IDisposable
```

Inheritance Object → ProfileSequenceDescriptor

Implements IDisposable

## Properties
## Context Property

```csharp
public Context Context { get; }
```

### Property Value

`Context`  
The `Context` supplied to create this instance.

---
## Handle Property

```csharp
public IntPtr Handle { get; }
```

### Property Value

`Handle`  
Gets the handle to the profile sequence descriptor.

---
## IsDisposed Property

```csharp
public bool IsDisposed { get; }
```

### Property Value

`IsDisposed`  
Gets a value indicating whether the instance has been disposed.

---
## Length Property

```csharp
public uint Length { get; }
```

### Property Value

`Length`  
Gets the number of profiles in the sequence.

## Methods
## Create(Context, uint) Method

```csharp
public static ProfileSequenceDescriptor Create(Context context, uint nItems)
```

Creates an instance of the `ProfileSequenceDescriptor` class.

### Parameters

`context` [Context](./Context)  
A `Context`, or null for the global context.

`nItems` uint  
The number of profiles in the sequence.

### Returns

`ProfileSequenceDescriptor`  
A new `ProfileSequenceDescriptor` instance.

---
## Dispose Method

```csharp
public void Dispose()
```

Disposes this instance.

---
## Duplicate() Method

```csharp
public ProfileSequenceDescriptor Duplicate()
```

Duplicates a profile sequence descriptor.

### Returns

`ProfileSequenceDescriptor`  
A new `ProfileSequenceDescriptor` instance.

---
## FromHandle(IntPtr) Method

```csharp
public static ProfileSequenceDescriptor FromHandle(IntPtr handle)
```

Creates a profile sequence descriptor from the supplied handle.

### Parameters

`handle` `IntPtr`  
A handle to an existing profile sequence descriptor.

### Returns

`ProfileSequenceDescriptor`  
A new `ProfileSequenceDescriptor` instance referencing an existing profile sequence descriptor.

---
## this[int] Method

```csharp
public ProfileSequenceItem this[int index]
```

Gets the `ProfileSequenceItem` at a given index position.

### Parameters

`index` int  
The index position.

### Returns

[ProfileSequenceItem](./ProfileSequenceItem.md)  
The item in the profile sequence.