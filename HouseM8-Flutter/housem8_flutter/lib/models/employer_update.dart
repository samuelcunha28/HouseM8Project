import 'address.dart';

class EmployerUpdate {
  String firstName;
  String lastName;
  String userName;
  String description;
  Address address;

  EmployerUpdate({
    this.firstName,
    this.lastName,
    this.userName,
    this.description,
    this.address,
  });

  factory EmployerUpdate.fromJson(Map<String, dynamic> json) {
    return EmployerUpdate(
      firstName: json["firstName"],
      lastName: json["lastName"],
      userName: json["userName"],
      description: json["description"],
      address: Address.fromJson(json["address"]),
    );
  }
}
