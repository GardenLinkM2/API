ClientDialog
---

**POST** api/syn
Body:
{
  "token": "string"
}
Resultat:
{
  "token": "string"
}

Contact
---

**POST** api/Contact/{id}
id: User.Id
Body:
{
  "firstMessage": "string"
}
Resultat:
{
  "id": "uuid",
  "contact": UserDto,
  "status": Status.Pending,
  "firstMessage": "string"
}

**GET** api/Contact/{id}
id: Contact.Id
Resultat:
{
  "id": "uuid",
  "contact": UserDto,
  "status": Status.Pending,
  "firstMessage": "string"
}

**GET** api/Contact
Resultat: *Mes contacts*, dépend du Token
[{
  "id": "uuid",
  "contact": UserDto,
  "status": Status.Pending,
  "firstMessage": "string"
}]

Gardens
---

**GET** api/Gardens?options&longi&lati&dist
options: OData
longi: double
lati: double
distance: int (km)
Resultat: 
[
  {
    "id": "uuid",
    "name": "string",
    "isReserved": false,
    "minUse": 0,
    "description": "string",
    "location": {
      "streetNumber": 0,
      "street": "string",
      "postalCode": 0,
      "city": "string",
      "longitudeAndLatitude": {
        "longitude": 0,
        "latitude": 0
      }
    },
    "owner": "uuid",
    "validation": Status.Pending,
    "criteria": {
      "locationTime": 0,
      "area": 0,
      "price": 0,
      "orientation": "Unset",
      "typeOfClay": "string",
      "equipments": true,
      "waterAccess": true,
      "directAccess": true
    },
    "photos": [
      {
        "fileName": "string",
        "path": "string"
      }
    ]
  }
]

**POST** api/Gardens
Body:
{
  "name": "string",
  "minUse": 0,
  "description": "string",
  "location": {
    "streetNumber": 0,
    "street": "string",
    "postalCode": 0,
    "city": "string",
  },
  "criteria": {
    "locationTime": 0,
    "area": 0,
    "price": 0,
    "orientation": "Unset",
    "typeOfClay": "string",
    "equipments": true,
    "waterAccess": true,
    "directAccess": true
  },
  "photos": [
    {
      "fileName": "string",
      "path": "string"
    }
  ]
}
Resultat:
{
  "id": "uuid",
  "name": "string",
  "isReserved": false,
  "minUse": 0,
  "description": "string",
  "location": {
    "streetNumber": 0,
    "street": "string",
    "postalCode": 0,
    "city": "string",
    "longitudeAndLatitude": {
      "longitude": 0,
      "latitude": 0
    }
  },
  "owner": "uuid",
  "validation": Status.Pending,
  "criteria": {
    "locationTime": 0,
    "area": 0,
    "price": 0,
    "orientation": "Unset",
    "typeOfClay": "string",
    "equipments": true,
    "waterAccess": true,
    "directAccess": true
  },
  "photos": [
    {
      "fileName": "string",
      "path": "string"
    }
  ]
}

**GET** api/Gardens/{id}
id: Garden.Id
Resultat:
{
  "id": "uuid",
  "name": "string",
  "isReserved": "boolean",
  "minUse": 0,
  "description": "string",
  "location": {
    "streetNumber": 0,
    "street": "string",
    "postalCode": 0,
    "city": "string",
    "longitudeAndLatitude": {
      "longitude": 0,
      "latitude": 0
    }
  },
  "owner": "uuid",
  "validation": Status.Pending,
  "criteria": {
    "locationTime": 0,
    "area": 0,
    "price": 0,
    "orientation": "Unset",
    "typeOfClay": "string",
    "equipments": true,
    "waterAccess": true,
    "directAccess": true
  },
  "photos": [
    {
      "fileName": "string",
      "path": "string"
    }
  ]
}

**POST** api/Gardens/coordinates
Body: Obligatoire: (1) && (2) && (3) || (3) && (4)
{
  "streetNumber": 0, (1)
  "street": "string", (2)
  "postalCode": 0, (3)
  "city": "string" (4)
}
Resultat:
{
  "longitude": 0,
  "latitude": 0
}

**POST** api/Gardens/{id}/report
id: Garden.Id
Body:
{
  "reason": "string",
  "comment": "string",
}
Resultat:
{
  "id": "uuid",
  "reason": "string",
  "comment": "string",
  "ofGarden": "uuid"
}

Leasing
---

**GET** api/Leasing/me
me: Basé sur le Token où je suis le renter ou le owner
Resultat:
[
  {
    "id": "uuid",
    "creation": Timestamp,
    "time": int, (nb de secondes)
    "begin": Timestamp,
    "end": Timestamp,
    "renew": "boolean",
    "state": LeasingStatus,
    "garden": "uuid",
    "renter": "uuid",
    "owner": "uuid"
  }
]

**GET** api/Leasing/{id}
id: Leasing.Id
Resultat:
{
  "id": "uuid",
  "creation": Timestamp,
  "time": int, (nb de secondes)
  "begin": Timestamp,
  "end": Timestamp,
  "renew": "boolean",
  "state": LeasingStatus,
  "garden": "uuid",
  "renter": "uuid",
  "owner": "uuid"
}

**POST** api/Leasing
Body:
{
  "begin": Timestamp,
  "end": Timestamp,
  "garden": "uuid",
}
Resultat:
{
  "id": "uuid",
  "creation": Timestamp,
  "time": int, (nb de secondes)
  "begin": Timestamp,
  "end": Timestamp,
  "renew": false,
  "state": LeasingStatus.Pending,
  "garden": "uuid",
  "renter": "uuid",
  "owner": "uuid"
}

Payments
---

**GET** api/Payments/me
me: Basé sur le Token
Resultat:
[
  {
    "id": "uuid",
    "sum": 0,
    "date": Timestamp,
    "leasing": "uuid"
  }
]

**GET** api/Payments/{id}
id: Payment.Id
Resultat:
{
  "id": "uuid",
  "sum": int,
  "date": Timestamp,
  "leasing": "uuid"
}


**POST** api/Payments
Body:
{
  "sum":int,
  "leasing": "uuid"
}
Resultat:
{
  "id": "uuid",
  "sum": int,
  "date": Timestamp,
  "leasing": "uuid"
}

Scores
---

**GET** api/Users/me/score
me: Basé sur le Token
Resultat:
[
  {
    "id": "uuid",
    "mark": "int",
    "comment": "string",
    "rater": "uuid",
    "rated": "uuid"
  }
]

**GET** api/Gardens/{id}/score
id: Garden.Id
Resultat:
[
  {
    "id": "uuid",
    "mark": "int",
    "comment": "string",
    "rater": "uuid",
    "rated": "uuid"
  }
]

**POST** api/Users/{id}/score
id: Garden.Id
Body:
{
  "mark": "int", (de 0 à 5)
  "comment": "string",
}
Resultat:
{
  "id": "uuid",
  "mark": "int",
  "comment": "string",
  "rater": "uuid",
  "rated": Url.id
}

**GET** api/Users/{id}/score
id: User.Id
Resultat:
[
  {
    "id": "uuid",
    "mark": "int",
    "comment": "string",
    "rater": "uuid",
    "rated": "uuid"
  }
]

**POST** api/Users/{id}/score
id: User.Id
Body:
{
  "mark": "int", (de 0 à 5)
  "comment": "string",
}
Resultat:
{
  "id": "uuid",
  "mark": "int",
  "comment": "string",
  "rater": "uuid",
  "rated": Url.id
}

**POST** api/score/{id}/report
id; Score.Id
Body:
No body
Resultat:
No result, 204Success

Talks
---

**GET** api/Talks
Dépend du Token
Resultat:
[
  {
    "id": "uuid",
    "subject": "string",
    "isArchived": false,
    "sender": "uuid",
    "receiver": "uuid",
    "messages": [
      MessageDto
    ]
  }
]

**POST** api/Talks
Body:
{
  "subject": "string",
  "receiver": "uuid",
}
Resultat:
{
  "id": "uuid",
  "subject": "string",
  "isArchived": false,
  "sender": "uuid",
  "receiver": "uuid",
}

**GET** api/Talks/{id}
id: Talk.Id
Resultat:
{
  "id": "uuid",
  "subject": "string",
  "isArchived": false,
  "sender": "uuid",
  "receiver": "uuid",
  "messages": [
    MessageDto
  ]
}

**POST** api/Talks/{id} (Pour envoyer un message)
id: Talk.Id
Body:
{
  "text": "string",
  "photos": [
    {
      "fileName": "string",
      "path": "string"
    }
  ]
}

Resultat:
{
  "id": "uuid",
  "text": "string",
  "creationDate": Timestamp,
  "isRead": false,
  "sender": "uuid",
  "photos": [
    {
      "fileName": "string",
      "path": "string"
    }
  ]
}

Users
---

**GET** api/Users/{id}/gardens
id: User.Id
Resultat:
[
	GardenDto
]

**GET** api/Users/me/gardens
me: Basé sur le Token
Resultat:
[
	GardenDto
]

Wallets
---

**GET** api/Wallets/me
me: Basé sur le Token
Resultat:
{
  "id": "uuid",
  "balance": 0
}