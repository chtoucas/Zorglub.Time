// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#if ENABLE_PREVIEW_FEATURES

[assembly: SuppressMessage("Usage", "CA2252:This API requires opting into preview features")]

#endif

#if FEATURE_STATIC_ABSTRACT
#if DEBUG
#warning Built with the preview feature "static abstract" enabled
#else
#error Built with the preview feature "static abstract" enabled
#endif
#endif
