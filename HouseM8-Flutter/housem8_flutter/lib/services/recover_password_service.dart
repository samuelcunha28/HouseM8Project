import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/models/recover_password.dart';
import 'package:http/http.dart' as https;

class RecoverPasswordService {
  Future<int> changePassword([RecoverPassword newPassword]) async {
    final url = DotEnv().env['REST_API_URL'] + "resetPassword";

    final response = await https.post(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      body: jsonEncode(<String, dynamic>{
        'email': newPassword.email,
        'password': newPassword.password,
        'confirmPassword': newPassword.confirmPassword,
        'token': newPassword.token,
      }),
    );

    return response.statusCode;
  }
}
