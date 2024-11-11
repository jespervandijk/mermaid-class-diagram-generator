classDiagram
class Auto{
  +Int32 Id
  +String Model
}

class Wheels{
  +Int32 Count
  +String Type
}

`Aggregate<T>` <|-- Auto
Auto o-- Wheels
