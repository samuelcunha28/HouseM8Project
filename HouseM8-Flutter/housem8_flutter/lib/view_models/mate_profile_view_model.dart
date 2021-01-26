import 'package:flutter/cupertino.dart';
import 'package:housem8_flutter/models/mate_update.dart';
import 'package:housem8_flutter/models/profile_model.dart';
import 'package:housem8_flutter/services/mate_profile_service.dart';

class MateProfileViewModel extends ChangeNotifier {
  ProfileModel mateProfile;

  mateProfileViewModelConstructor() async {
    this.mateProfile = await MateProfileService().fetchMateProfile();
    notifyListeners();
  }

  MateProfileViewModel() {
    mateProfileViewModelConstructor();
  }

  Future<void> updateMateProfile([MateUpdate mate]) async {
    mateProfile.firstName = mate.firstName;
    mateProfile.lastName = mate.lastName;
    mateProfile.userName = mate.userName;
    mateProfile.address = mate.address.toString();
    mateProfile.description = mate.description;
    notifyListeners();
    await MateProfileService().putMateUpdate(mate);
  }

  int get id {
    return this.mateProfile.id;
  }

  String get firstName {
    return this.mateProfile.firstName;
  }

  set firstName(String firstName) {
    this.mateProfile.firstName = firstName;
  }

  String get lastName {
    return this.mateProfile.lastName;
  }

  set lastName(String lastName) {
    this.mateProfile.lastName = lastName;
  }

  String get userName {
    return this.mateProfile.userName;
  }

  set userName(String userName) {
    this.mateProfile.userName = userName;
  }

  String get address {
    return this.mateProfile.address;
  }

  set address(String address) {
    this.mateProfile.address = address;
  }

  String get description {
    if (this.mateProfile.description != null) {
      return this.mateProfile.description;
    } else {
      return "Sem descrição";
    }
  }

  set description(String description) {
    this.mateProfile.description = description;
  }

  double get averageRating {
    return this.mateProfile.averageRating;
  }
}
