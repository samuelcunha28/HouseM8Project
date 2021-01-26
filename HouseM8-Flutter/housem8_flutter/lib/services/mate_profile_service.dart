import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/mate_update.dart';
import 'package:housem8_flutter/models/profile_model.dart';
import 'package:http/http.dart' as http;

class MateProfileService {
  Future<ProfileModel> fetchMateProfile() async {
    final String id = await StorageHelper.readTokenID();

    final url = DotEnv().env['REST_API_URL'] + "mateprofile/" + id;

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return ProfileModel.fromJson(body);
    } else {
      throw Exception("Request Failed");
    }
  }

  Future<void> putMateUpdate([MateUpdate newMate]) async {
    final url = DotEnv().env['REST_API_URL'] + "mateprofile/update";

    final String token = await StorageHelper.readToken();

    final response = await http.put(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer $token',
      },
      body: jsonEncode(<String, dynamic>{
        'firstName': newMate.firstName,
        'lastName': newMate.lastName,
        'userName': newMate.userName,
        'description': newMate.description,
        'address': newMate.address.toJson(),
        'range': newMate.range,
      }),
    );

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }
}
