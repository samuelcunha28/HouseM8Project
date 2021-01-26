import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:http/http.dart' as http;

class LoginService {
  static final String jwtTokenKey = "JWT";
  final storage = FlutterSecureStorage();

  Future<int> loginUser(String email, String password) async {
    final url = DotEnv().env['REST_API_URL'] + "login";
    final body = jsonEncode({"email": email, "password": password});

    final response = await http.post(url, body: body, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    });

    if (response.statusCode == 200) {
      StorageHelper.storeToken(jsonDecode(response.body)["token"]);
      StorageHelper.storeTokenData(ascii.decode(base64.decode(
          base64.normalize(jsonDecode(response.body)["token"].split(".")[1]))));
      return response.statusCode;
    } else {
      return response.statusCode;
    }
  }
}
