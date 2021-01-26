import 'package:housem8_flutter/models/address.dart';

class MateUpdate {
  String firstName;
  String lastName;
  String userName;
  String description;
  Address address;
  int range;

  MateUpdate(
      {this.firstName,
      this.lastName,
      this.userName,
      this.description,
      this.address,
      this.range});

  factory MateUpdate.fromJson(Map<String, dynamic> json) {
    return MateUpdate(
      firstName: json["firstName"],
      lastName: json["lastName"],
      userName: json["userName"],
      description: json["description"],
      address: Address.fromJson(json["address"]),
      range: json["range"],
    );
  }
}
