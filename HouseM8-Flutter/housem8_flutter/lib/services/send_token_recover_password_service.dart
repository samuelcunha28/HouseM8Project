import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/models/forgot_password.dart';
import 'package:http/http.dart' as https;

class SendTokenService {
  Future<int> sendToken([ForgotPassword newPassword]) async {
    final url = DotEnv().env['REST_API_URL'] + "forgotPassword";

    final response = await https.post(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      body: jsonEncode(<String, dynamic>{
        'email': newPassword.email,
      }),
    );

    return response.statusCode;
  }
}
