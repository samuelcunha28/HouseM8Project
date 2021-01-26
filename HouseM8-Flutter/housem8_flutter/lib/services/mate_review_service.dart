import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/mate_review.dart';
import 'package:http/http.dart' as https;

class MateReviewService {
  Future<int> createMateReview(int mateId, [MateReviews mateReview]) async {
    final url = DotEnv().env['REST_API_URL'] + "Reviews/mate/$mateId";

    final String token = await StorageHelper.readToken();

    final response = await https.post(url,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(<String, dynamic>{
          'rating': mateReview.rating,
          'comment': mateReview.comment,
        }));

    if (response.statusCode != 200) {
      print(response.body);
      throw Exception("Request Failed");
    }

    return response.statusCode;
  }

  Future<List<MateReviews>> fetchMateReviews() async {
    final url = DotEnv().env['REST_API_URL'] + "Reviews/matereviews";

    final String token = await StorageHelper.readToken();

    final response = await https.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      final Iterable json = body;
      return json
          .map((reviewToMate) => MateReviews.fromJson(reviewToMate))
          .toList();
    } else {
      return List<MateReviews>();
    }
  }
}
