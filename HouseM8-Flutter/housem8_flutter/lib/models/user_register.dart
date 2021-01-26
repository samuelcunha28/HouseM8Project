import 'package:housem8_flutter/models/address.dart';

class UserRegister {
  final String firstName;
  final String lastName;
  final String userName;
  final String password;
  final String email;
  final String description;
  final Address address;

  UserRegister(
      {this.firstName,
      this.lastName,
      this.userName,
      this.password,
      this.email,
      this.description,
      this.address});

  factory UserRegister.fromJson(Map<String, dynamic> json) {
    return UserRegister(
      firstName: json["firstName"],
      lastName: json["lastName"],
      userName: json["userName"],
      password: json["password"],
      email: json["email"],
      description: json["description"],
      address: Address.fromJson(json["address"]),
    );
  }
}
