classDiagram
class Auto{
  +Int32 Id
  +String Model
}

class Entity

class Wheels{
  +Int32 Count
  +String Type
}

Entity <|-- Wheels
Entity <|-- Auto
Auto o-- Wheels
