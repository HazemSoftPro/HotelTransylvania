# User Story: استبدال التسعير الأساسي بالتسعير اليدوي

## User Story

**كمدير فندق، أريد أن أتمكن من تحديد سعر مخصص لكل غرفة على حدة بدلاً من الاعتماد على سعر أساسي موحد لنوع الغرفة، حتى أتمكن من مرونة أكبر في التسعير وإدارة الإيرادات.**

## Background

حالياً النظام يستخدم نموذج تسعير من مستويين:
- `basePrice` في `RoomType` (سعر أساسي لكل نوع غرفة)
- `priceOverride` في `Room` (اختياري لتجاوز السعر الأساسي)

المطلوب تبسيط هذا النموذج إلى مستوى واحد:
- `manualPrice` في `Room` (سعر إلزامي لكل غرفة)

## Acceptance Criteria

### ✅ Domain Model Changes

**AC1: إزالة BasePrice من RoomType**
- [ ] إزالة `BasePrice` property من `RoomType` entity
- [ ] إزالة كل الـ validation rules المتعلقة بـ `basePrice`
- [ ] إزالة `basePrice` parameter من constructor و `UpdateDetails` method

**AC2: استبدال PriceOverride بـ ManualPrice في Room**
- [ ] إزالة `PriceOverride` property من `Room` entity
- [ ] إضافة `ManualPrice` property (decimal, required, > 0)
- [ ] تحديث constructor و `UpdateDetails` method لاستخدام `manualPrice`
- [ ] إضافة validation: `ManualPrice` يجب أن يكون > 0 دائماً

### ✅ Database Migration

**AC3: ترحيل البيانات بأمان**
- [ ] إنشاء migration script يقوم بـ:
  - إضافة `ManualPrice` column إلى `Rooms` table
  - نسخ `basePrice` من `RoomTypes` إلى `ManualPrice` في `Rooms`
  - إزالة `BasePrice` column من `RoomTypes` table
  - إزالة `PriceOverride` column من `Rooms` table
- [ ] إضافة check constraint: `ManualPrice > 0`
- [ ] اختبار Migration على بيانات تجريبية
- [ ] إنشاء rollback script للطوارئ

### ✅ API Changes

**AC4: تحديث DTOs والـ Use Cases**
- [ ] تحديث `RoomDTO` لإزالة `BasePrice` وإضافة `ManualPrice`
- [ ] تحديث `RoomTypeDTO` لإزالة `BasePrice`
- [ ] تحديث كل الـ Use Cases (Create, Update, Get, List, Search)
- [ ] إزالة كل المنطق المتعلق بحساب الأسعار باستخدام `basePrice`

**AC5: تحديث API Endpoints**
- [ ] تعديل Room endpoints لقبول `manualPrice` بدلاً من `priceOverride`
- [ ] تعديل RoomType endpoints لإزالة `basePrice`
- [ ] تحديث validation rules: `manualPrice` مطلوب وأكبر من الصفر
- [ ] تحديث API responses لإرجاع `manualPrice`

### ✅ Frontend Changes

**AC6: تحديث TypeScript Types**
- [ ] تحديث `RoomResponse` interface لإزالة `basePrice` وإضافة `manualPrice`
- [ ] تحديث `UpdateRoomRequest` interface لاستخدام `manualPrice`
- [ ] إزالة `basePrice` من كل الـ service interfaces

**AC7: تحديث UI Components**
- [ ] تحديث `RoomCard` لعرض `manualPrice` بدلاً من `basePrice`
- [ ] تحديث `RoomForm` لتحرير `manualPrice`
- [ ] تحديث `ReservationForm` لاستخدام `manualPrice` في الحسابات
- [ ] تحديث `RoomDetails` لعرض `manualPrice`
- [ ] إضافة proper error handling للـ validation الجديد

### ✅ Testing

**AC8: Unit Tests**
- [ ] اختبار `Room` entity مع `ManualPrice` validation
- [ ] اختبار `RoomType` entity بدون `BasePrice`
- [ ] اختبار كل الـ Use Cases المحدثة
- [ ] اختبار API controllers مع التسعير الجديد

**AC9: Integration Tests**
- [ ] اختبار API endpoints مع `manualPrice`
- [ ] اختبار Migration script على database تجريبي
- [ ] اختبار Frontend components مع البيانات الجديدة

### ✅ Data Integrity

**AC10: ضمان سلامة البيانات**
- [ ] كل غرفة موجودة يجب أن تحصل على `manualPrice` صالح من `basePrice` الخاص بـ `RoomType`
- [ ] لا يجب فقدان أي بيانات أثناء الترحيل
- [ ] كل `manualPrice` يجب أن يكون > 0
- [ ] إزالة كل المراجع لـ `basePrice` و `priceOverride` من الكود

### ✅ Documentation

**AC11: تحديث التوثيق**
- [ ] تحديث API documentation
- [ ] تحديث database schema documentation
- [ ] إنشاء migration guide للمطورين
- [ ] تحديث `SeedData.cs` لاستخدام `ManualPrice`

## Definition of Done

- [ ] كل الـ Acceptance Criteria مكتملة
- [ ] كل الاختبارات تمر بنجاح (Unit + Integration + Frontend)
- [ ] Migration script مختبر ويعمل بدون أخطاء
- [ ] API endpoints تعمل مع التسعير الجديد
- [ ] Frontend يعرض ويحرر `manualPrice` بشكل صحيح
- [ ] لا توجد مراجع لـ `basePrice` أو `priceOverride` في الكود
- [ ] Code review مكتمل
- [ ] Documentation محدث

## Risk Mitigation

### المخاطر المحتملة:
1. **فقدان البيانات أثناء Migration** - تم تخفيفه بـ backup procedures و rollback script
2. **كسر API compatibility** - تم تخفيفه بـ comprehensive testing
3. **Frontend errors** - تم تخفيفه بـ proper error handling و validation
4. **Performance impact** - تم تخفيفه بـ database indexing و query optimization

### خطة الطوارئ:
- Rollback script جاهز للتراجع عن Migration
- Feature flags للتبديل بين النماذج القديمة والجديدة
- Monitoring و alerting للكشف عن المشاكل مبكراً

## Success Metrics

- [ ] 100% من الغرف لديها `manualPrice` صالح
- [ ] 0 errors في API endpoints بعد التحديث
- [ ] 0 broken UI components
- [ ] Migration time < 5 minutes
- [ ] All tests pass (target: 100% success rate)
