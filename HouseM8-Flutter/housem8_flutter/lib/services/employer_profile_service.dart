import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/employer_update.dart';
import 'package:housem8_flutter/models/profile_model.dart';
import 'package:http/http.dart' as http;

class EmployerProfileService {
  Future<ProfileModel> fetchEmployerProfile() async {
    final String token = await StorageHelper.readToken();

    final String id = await StorageHelper.readTokenID();

    final url = DotEnv().env['REST_API_URL'] + "employerprofile/" + id;

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return ProfileModel.fromJson(body);
    } else {
      throw Exception("Request Failed");
    }
  }

  Future<void> updateEmployer([EmployerUpdate newEmployer]) async {
    final url = DotEnv().env['REST_API_URL'] + "employerprofile/update";

    final String token = await StorageHelper.readToken();

    final response = await http.put(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer $token',
      },
      body: jsonEncode(<String, dynamic>{
        'firstName': newEmployer.firstName,
        'lastName': newEmployer.lastName,
        'userName': newEmployer.userName,
        'description': newEmployer.description,
        'address': newEmployer.address.toJson(),
      }),
    );

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }
}
