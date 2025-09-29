type Branded<T, B extends string> = T & { __brand: B };

type Name = Branded<string, "Name">;
type Surname = Branded<string, "Surname">;
type Phone = Branded<string, "Phone">;
type Email = Branded<string, "Email">;
type Address = Branded<string, "Address">;
type City = Branded<string, "City">;
type Country = Branded<string, "Country">;
type ZipCode = Branded<string, "ZipCode">;
type Description = Branded<string, "Description">;

interface Guest {
  id: number;
  name: Name;
  surname: Surname;
  phone: Phone;
  email: Email;
  address: Address;
  city: City;
  country: Country;
  zip_code: ZipCode;
  description: Description;
}

export {
  Guest,
  Name,
  Surname,
  Phone,
  Email,
  Address,
  City,
  Country,
  ZipCode,
  Description,
};
