import 'package:housem8_flutter/models/mate.dart';

class MateViewModel {
  final Mate mate;

  MateViewModel({this.mate});

  int get userId {
    return this.mate.id;
  }

  String get userName {
    return this.mate.userName;
  }

  int get range {
    return this.mate.range;
  }
}
