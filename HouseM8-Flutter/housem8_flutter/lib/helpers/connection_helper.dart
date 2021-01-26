import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:http/http.dart' as http;

class ConnectionHelper {
  static Future<bool> checkConnection() async {
    return await Connectivity().checkConnectivity().then((value) async {
      if (value != ConnectivityResult.none) {
        return await DataConnectionChecker().hasConnection;
      } else {
        return false;
      }
    });
  }

  static Future<int> checkTokenValidaty() async {
    final url = DotEnv().env['REST_API_URL'] +
        "location?lat=41.3085168113476&lng=-8.344125348737023";

    final String token = await StorageHelper.readToken();

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      return response.statusCode;
    } else if (response.statusCode == 401) {
      await StorageHelper.deleteAllTokenData();
      return response.statusCode;
    } else {
      return response.statusCode;
    }
  }
}
