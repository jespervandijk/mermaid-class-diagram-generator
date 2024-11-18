classDiagram
class Auto{
  +Int32 Id
  +String Model
}

class Wheels{
  +Int32 Count
  +String Type
}

class `Aggregate<T>`

Auto o-- Wheels
`Aggregate<T>` <|-- Auto
