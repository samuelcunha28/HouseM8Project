import 'dart:async';
import 'dart:convert';

import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/job_post.dart';
import 'package:housem8_flutter/models/job_post_publication.dart';
import 'package:housem8_flutter/models/main_image.dart';
import 'package:http/http.dart' as http;

class JobPostWebService {
  Future<List<JobPost>> fetchJobPosts() async {
    final url = DotEnv().env['REST_API_URL'] +
        "worksearch?categories=PLUMBING&categories=GARDENING&categories=CLEANING&distance=400";

    final String token = await StorageHelper.readToken();

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      final Iterable json = body;
      return json.map((jobPost) => JobPost.fromJson(jobPost)).toList();
    } else {
      return List<JobPost>();
    }
  }

  Future<List<JobPost>> fetchEmployerPosts() async {
    final url = DotEnv().env['REST_API_URL'] + "JobPosts/EmployerPosts";

    final String token = await StorageHelper.readToken();

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      final Iterable json = body;
      return json.map((jobPost) => JobPost.fromJson(jobPost)).toList();
    } else {
      return List<JobPost>();
    }
  }

  Future<MainImage> fetchMainImage(int jobPost) async {
    final url =
        DotEnv().env['REST_API_URL'] + "JobPosts/postMainImage/$jobPost";

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

  Future<void> ignoreJobPost(int id) async {
    final url = DotEnv().env['REST_API_URL'] + "WorkSearch/ignoreJobPost";

    final String token = await StorageHelper.readToken();

    final response = await http.post(url,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(<String, int>{
          'id': id,
        }));

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }

  Future<void> makeOffer(int id, double price) async {
    final url = DotEnv().env['REST_API_URL'] + "WorkSearch/makeOffer/$id";

    final String token = await StorageHelper.readToken();

    final response = await http.post(url,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(<String, double>{
          'price': price,
        }));

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }

  Future<void> createJobPost(JobPostPublication jobPost) async {
    final url = DotEnv().env['REST_API_URL'] + "JobPosts/CreatePost";

    final String token = await StorageHelper.readToken();

    final response = await http.post(url,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(<String, dynamic>{
          'title': jobPost.title,
          'category': EnumToString.convertToString(jobPost.category),
          'description': jobPost.description,
          'tradable': jobPost.tradable,
          'initialPrice': jobPost.initialPrice,
          'address': jobPost.address.toJson(),
          'paymentMethod': jobPost.paymentMethod
              .map((payment) => EnumToString.convertToString(payment))
              .toList(),
        }));

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }

  Future<void> deleteJobPost(int id) async {
    final url = DotEnv().env['REST_API_URL'] + "JobPosts/DeletePost/$id";

    final String token = await StorageHelper.readToken();

    final response = await http.delete(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }

  Future<void> updateJobPost(JobPostPublication jobPost, int id) async {
    final url = DotEnv().env['REST_API_URL'] + "jobposts/updatePost/$id";

    final String token = await StorageHelper.readToken();

    final response = await http.put(url,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(<String, dynamic>{
          'title': jobPost.title,
          'category': EnumToString.convertToString(jobPost.category),
          'description': jobPost.description,
          'tradable': jobPost.tradable,
          'initialPrice': jobPost.initialPrice,
          'address': jobPost.address.toJson(),
          'paymentMethod': jobPost.paymentMethod
              .map((payment) => EnumToString.convertToString(payment))
              .toList(),
        }));

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }
}
