import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/models/employer_register.dart';
import 'package:http/http.dart' as https;

class RegisterEmployerService {
  Future<int> createEmployerRegister([EmployerRegister newEmployer]) async {
    final url = DotEnv().env['REST_API_URL'] + "Users/employer";

    final response = await https.post(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      body: jsonEncode(<String, dynamic>{
        'firstName': newEmployer.firstName,
        'lastName': newEmployer.lastName,
        'userName': newEmployer.userName,
        'password': newEmployer.password,
        'email': newEmployer.email,
        'description': newEmployer.description,
        'address': newEmployer.address.toJson(),
      }),
    );

    return response.statusCode;
  }
}
