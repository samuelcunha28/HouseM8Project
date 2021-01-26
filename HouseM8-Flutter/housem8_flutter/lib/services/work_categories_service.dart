import 'dart:async';
import 'dart:convert';

import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/work_categories.dart';
import 'package:http/http.dart' as http;

class WorkCategoriesService {
  Future<List<WorkCategories>> fetchWorkCategories() async {
    final url = DotEnv().env['REST_API_URL'] + "mateprofile/listcat";

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
          .map((workCategory) => WorkCategories.fromJson(workCategory))
          .toList();
    } else {
      throw Exception("Request Failed");
    }
  }

  Future<void> addCategory([WorkCategories category]) async {
    final url = Uri.parse(DotEnv().env['REST_API_URL'] + "MateProfile/addcat");
    final List<WorkCategories> listCategories = List<WorkCategories>();
    listCategories.add(category);

    final String token = await StorageHelper.readToken();

    final response = await http.post(url,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(listCategories));

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }

  Future<void> deleteCategory([WorkCategories category]) async {
    final url =
        Uri.parse(DotEnv().env['REST_API_URL'] + "mateprofile/deletecat");

    final String token = await StorageHelper.readToken();

    final request = http.Request("DELETE", url);

    request.headers.addAll(<String, String>{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    request.body = jsonEncode(
        {'categories': EnumToString.convertToString(category.category)});

    final response = await request.send();

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }
}
