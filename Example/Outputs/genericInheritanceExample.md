classDiagram
class Car{
  +Int32 Id
  +String Model
}

class Wheels{
  +Int32 Count
  +String Type
}

class `Aggregate<T>`

Car o-- Wheels
`Aggregate<T>` <|-- Car
