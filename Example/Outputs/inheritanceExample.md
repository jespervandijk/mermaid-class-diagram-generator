classDiagram
class Car{
  +Int32 Id
  +String Model
}

class Wheels{
  +Int32 Count
  +String Type
}

class Entity

Entity <|-- Wheels
Car o-- Wheels
Entity <|-- Car
