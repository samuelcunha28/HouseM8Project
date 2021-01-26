import 'package:housem8_flutter/enums/categories.dart';
import 'package:housem8_flutter/models/address.dart';
import 'package:housem8_flutter/models/user_register.dart';

class MateRegister extends UserRegister {
  final List<Categories> category;
  final int range;

  MateRegister(
      {String firstName,
      String lastName,
      String userName,
      String password,
      String email,
      String description,
      Address address,
      this.category,
      this.range})
      : super(
            firstName: firstName,
            lastName: lastName,
            userName: userName,
            password: password,
            email: email,
            description: description,
            address: address);
}
