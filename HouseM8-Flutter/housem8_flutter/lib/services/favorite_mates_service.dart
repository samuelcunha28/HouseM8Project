import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/favorite_mates.dart';
import 'package:http/http.dart' as http;

class FavoriteMatesService {
  Future<List<FavoriteMates>> fetchFavoriteMates() async {
    final url = DotEnv().env['REST_API_URL'] + "employerprofile/favlist";

    final String token = await StorageHelper.readToken();

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      final Iterable json = body;
      return json
          .map((favoriteMate) => FavoriteMates.fromJson(favoriteMate))
          .toList();
    } else {
      return List<FavoriteMates>();
    }
  }

  Future<void> deleteFavorite([FavoriteMates mate]) async {
    final url =
        Uri.parse(DotEnv().env['REST_API_URL'] + "employerprofile/delfav");

    final String token = await StorageHelper.readToken();

    final request = http.Request("DELETE", url);

    request.headers.addAll(<String, String>{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    request.body = jsonEncode({
      'userName': mate.name,
      'email': mate.email,
    });

    final response = await request.send();

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }
}
