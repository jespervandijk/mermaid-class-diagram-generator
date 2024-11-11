classDiagram
class Auto{
  +Int32 Id
  +String Model
}

class Wheels{
  +Int32 Count
  +String Type
}

Entity <|-- Auto
Entity <|-- Wheels
Auto o-- Wheels
