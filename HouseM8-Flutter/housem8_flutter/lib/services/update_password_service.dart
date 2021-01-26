import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/password_update.dart';
import 'package:http/http.dart' as http;

class UpdatePasswordService {
  Future<int> updatePassword([PasswordUpdate newPassword]) async {
    final url = DotEnv().env['REST_API_URL'] + "users/password";

    final String token = await StorageHelper.readToken();

    final response = await http.put(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer $token',
      },
      body: jsonEncode(<String, dynamic>{
        'password': newPassword.password,
        'oldPassword': newPassword.oldPassword,
      }),
    );
    return response.statusCode;
  }
}
