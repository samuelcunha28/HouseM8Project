import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/main_image.dart';
import 'package:housem8_flutter/models/mate.dart';
import 'package:http/http.dart' as http;

class MateSearchWebService {
  Future<List<Mate>> fetchMates() async {
    final url = DotEnv().env['REST_API_URL'] + "MateProfile/filterMates?";

    final String token = await StorageHelper.readToken();

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      final Iterable json = body;
      return json.map((mate) => Mate.fromJson(mate)).toList();
    } else {
      return List<Mate>();
    }
  }

  Future<MainImage> fetchProfileImage(int mateID) async {
    final url = DotEnv().env['REST_API_URL'] + "Users/profilePic/$mateID";
    final String token = await StorageHelper.readToken();

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return MainImage.fromJson(body);
    } else {
      return null;
    }
  }
}
