import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/mate_reviews.dart';
import 'package:http/http.dart' as http;

class ReviewsService {
  Future<List<Reviews>> fetchMateReviews() async {
    final url = DotEnv().env['REST_API_URL'] + "Reviews/matereviews";

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
          .map((reviewToMate) => Reviews.fromJson(reviewToMate))
          .toList();
    } else {
      return List<Reviews>();
    }
  }

  Future<int> fetchEmployerId(int jobId) async {
    final String token = await StorageHelper.readToken();

    final url = DotEnv().env['REST_API_URL'] + "Work/find/$jobId";

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return body['employer']['id'] as int;
    } else {
      throw Exception("Request Failed");
    }
  }

  Future<int> fetchMateId(int jobId) async {
    final String token = await StorageHelper.readToken();

    final url = DotEnv().env['REST_API_URL'] + "Work/find/$jobId";

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return body['mate']['id'] as int;
    } else {
      throw Exception("Request Failed");
    }
  }
}
