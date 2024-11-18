classDiagram
class Auto{
  +Int32 Id
  +String Model
}

class Wheels{
  +Int32 Count
  +String Type
}

class Entity

Entity <|-- Wheels
Auto o-- Wheels
Entity <|-- Auto
