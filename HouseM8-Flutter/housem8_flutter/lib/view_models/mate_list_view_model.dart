import 'package:flutter/cupertino.dart';
import 'package:housem8_flutter/services/mate_search_web_service.dart';

import 'mate_view_model.dart';

class MateListViewModel extends ChangeNotifier {
  List<MateViewModel> mates = List<MateViewModel>();

  Future<void> fetchMates() async {
    final returned = await MateSearchWebService().fetchMates();
    this.mates = returned.map((mate) => MateViewModel(mate: mate)).toList();
    notifyListeners();
  }
}
