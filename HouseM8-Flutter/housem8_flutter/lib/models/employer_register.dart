import 'package:housem8_flutter/models/address.dart';
import 'package:housem8_flutter/models/user_register.dart';

class EmployerRegister extends UserRegister {
  EmployerRegister(
      {String firstName,
      String lastName,
      String userName,
      String password,
      String email,
      String description,
      Address address})
      : super(
            firstName: firstName,
            lastName: lastName,
            userName: userName,
            password: password,
            email: email,
            description: description,
            address: address);

  factory EmployerRegister.fromJson(Map<String, dynamic> json) {
    return EmployerRegister(
        firstName: json["firstName"],
        lastName: json["lastName"],
        userName: json["userName"],
        password: json["password"],
        email: json["email"],
        description: json["description"],
        address: json["address"]);
  }
}
