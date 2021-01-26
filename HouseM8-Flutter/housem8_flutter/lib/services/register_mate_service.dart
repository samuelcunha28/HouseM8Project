import 'dart:async';
import 'dart:convert';

import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/models/mate_register.dart';
import 'package:http/http.dart' as https;

class RegisterMateService {
  Future<int> createMateRegister([MateRegister newMate]) async {
    final url = DotEnv().env['REST_API_URL'] + "Users/mate";

    final response = await https.post(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      body: jsonEncode(<String, dynamic>{
        'firstName': newMate.firstName,
        'lastName': newMate.lastName,
        'userName': newMate.userName,
        'password': newMate.password,
        'email': newMate.email,
        'description': newMate.description,
        'address': newMate.address.toJson(),
        'categories': newMate.category
            .map((category) => EnumToString.convertToString(category))
            .toList(),
        'range': newMate.range
      }),
    );

    return response.statusCode;
  }
}
